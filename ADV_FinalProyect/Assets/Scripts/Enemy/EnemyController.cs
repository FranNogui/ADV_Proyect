using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

enum EnemyState
{
    PATROLLING,
    SEARCHING,
    ATTACKING,
    FOLLOWING_PLAYER
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] SightTrigger sight;
    [SerializeField] NearTrigger near;
    [SerializeField] float alertnessFactor;
    [SerializeField] float alertnessLossFactor;
    [SerializeField] Light sightLight;
    [SerializeField] Light pointLight;
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] Transform[] lookDirectionOnPoints;
    [SerializeField] float[] waitingTimes;
    [SerializeField] Animator animator;
    [SerializeField] GameObject muzzle;
    [SerializeField] AudioSource burstFireSound;
    [SerializeField] AudioSource detectedAlarm;
    [SerializeField] GameObject text;
    PlayerController _player;
    EnemyManager _manager;
    EnemyState _state = EnemyState.PATROLLING;
    int _patrolID;
    bool _waiting;
    NavMeshAgent _agent;
    float _alertness;
    bool _canShot;

    private void Start()
    {
        _alertness = 0.0f;
        _canShot = true;
        _agent = GetComponent<NavMeshAgent>();
        _patrolID = 0;
        _manager = FindObjectOfType<EnemyManager>();
        _player = FindObjectOfType<PlayerController>();
        _manager.AddEnemy(this);
        muzzle.SetActive(false);
    }

    private void Update()
    {
        text.transform.LookAt(Camera.main.transform);
        animator.SetFloat("velocity", Mathf.LerpUnclamped(animator.GetFloat("velocity"), Mathf.Abs(_agent.velocity.magnitude) / _agent.speed, Time.deltaTime * 20.0f));
        switch (_state)
        {
            case EnemyState.PATROLLING:
                if (CheckPlayerInSight())
                {
                    detectedAlarm.Play();
                    Vector3 playerPos = near.CanSeePlayer ? near.Player.transform.position : sight.Player.transform.position;
                    float distance = Mathf.Abs((this.transform.position - playerPos).magnitude);
                    _alertness += alertnessFactor * (1 / (distance * distance)) * Time.deltaTime;
                    var lookPos = playerPos - transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10.0f);
                    animator.SetBool("playerOnSight", true);
                    _agent.isStopped = true;
                    _state = EnemyState.SEARCHING;
                    break;
                }
                else
                {
                    text.SetActive(false);
                    if (!_waiting && patrolPoints.Length > 0.0f)
                    {
                        _agent.destination = patrolPoints[_patrolID].position;
                        _agent.isStopped = false;
                        transform.forward = Vector3.Lerp(transform.forward, _agent.velocity, Time.deltaTime * 5.0f);
                        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance && (!_agent.hasPath || Mathf.Approximately(_agent.velocity.sqrMagnitude, 0f)))
                            StartCoroutine(WaitOnPoint());
                    }
                    else if (_waiting && patrolPoints.Length > 0.0f)
                    {
                        var lookPos = lookDirectionOnPoints[_patrolID].position - transform.position;
                        lookPos.y = 0;
                        var rotation = Quaternion.LookRotation(lookPos);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5.0f);
                    }
                }
                break;
            case EnemyState.SEARCHING:
                {
                    text.SetActive(true);
                    TextMeshProUGUI textContent = text.GetComponentInChildren<TextMeshProUGUI>();
                    textContent.text = "?";
                    textContent.color = new Color(0, 219, 255);
                    textContent.outlineColor = new Color(0, 86, 94);
                }
                if (CheckPlayerInSight())
                {
                    Vector3 playerPos = near.CanSeePlayer ? near.Player.transform.position : sight.Player.transform.position;
                    float distance = Mathf.Abs((this.transform.position - playerPos).magnitude);
                    _alertness += alertnessFactor * (1 / (distance * distance)) * Time.deltaTime;
                    var lookPos = playerPos - transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10.0f);
                    animator.SetBool("playerOnSight", true);
                    _agent.isStopped = true;
                }
                else
                {
                    _alertness -= alertnessLossFactor * Time.deltaTime;
                }

                _alertness = Mathf.Clamp(_alertness, 0.0f, 100.0f);

                if (Mathf.Approximately(_alertness, 0.0f))
                {
                    _state = EnemyState.PATROLLING;
                    animator.SetBool("playerOnSight", false);
                    _alertness = 0.0f;
                }
                else if (Mathf.Approximately(_alertness, 100.0f))
                {
                    _manager.LastPlayerPosition = near.CanSeePlayer ? near.Player.transform.position : sight.Player.transform.position;
                    _manager.PlayerFound();
                    _state = EnemyState.ATTACKING;
                    _alertness = 100.0f;
                }

                sightLight.color = Color.Lerp(Color.green, Color.red, _alertness / 100.0f);
                pointLight.color = Color.Lerp(Color.green, Color.red, _alertness / 100.0f);
                break;
            case EnemyState.ATTACKING:
                {
                    TextMeshProUGUI textContent = text.GetComponentInChildren<TextMeshProUGUI>();
                    textContent.text = "!";
                    textContent.color = new Color(255, 15, 0);
                    textContent.outlineColor = new Color(255, 15, 0);
                }
                if (!CheckPlayerInSight())
                    _state = EnemyState.FOLLOWING_PLAYER;
                else
                {
                    if (_canShot)
                        StartCoroutine(shoot());
                    Vector3 position = sight.CanSeePlayer ? sight.Player.transform.position : near.Player.transform.position;
                    _manager.LastPlayerPosition = position;
                    Vector3 playerPos = near.CanSeePlayer ? near.Player.transform.position : sight.Player.transform.position;
                    float distance = Mathf.Abs((this.transform.position - playerPos).magnitude);
                    var lookPos = playerPos - transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10.0f);
                    animator.SetBool("playerOnSight", true);
                    if (distance < 5.0f)
                        _agent.isStopped = true;
                    else
                    {
                        _agent.destination = playerPos;
                        _agent.isStopped = false;
                    }
                }
                break;
            case EnemyState.FOLLOWING_PLAYER:
                _agent.destination = _manager.LastPlayerPosition;
                _agent.isStopped = false;
                transform.forward = Vector3.Lerp(transform.forward, _agent.velocity, Time.deltaTime * 5.0f);
                if (CheckPlayerInSight())
                    _state = EnemyState.ATTACKING;
                else if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance && (!_agent.hasPath || Mathf.Approximately(_agent.velocity.sqrMagnitude, 0f)))
                    _manager.PlayerLost();
                break;
        }
    }

    bool CheckPlayerInSight()
    {
        if (sight.CanSeePlayer || near.CanSeePlayer)
        {
            Vector3 start = new Vector3(transform.position.x, transform.position.y + 2.0f, transform.position.z);
            Vector3 end;
            if (sight.CanSeePlayer)
                end = sight.Player.transform.position;
            else
                return true;
            return !Physics.Linecast(start, end);
        }

        return false;
    }

    IEnumerator WaitOnPoint()
    {
        _waiting = true;
        yield return new WaitForSeconds(waitingTimes[_patrolID]);
        _waiting = false;
        _patrolID += 1;
        _patrolID %= patrolPoints.Length;
    }

    public void PlayerFound()
    {
        if (_state != EnemyState.ATTACKING)
            _state = EnemyState.FOLLOWING_PLAYER;
        _agent.speed = 4.0f;
        _agent.stoppingDistance = 1.0f;
        _alertness = 100.0f;
        sightLight.color = Color.Lerp(Color.green, Color.red, _alertness / 100.0f);
        pointLight.color = Color.Lerp(Color.green, Color.red, _alertness / 100.0f);
        animator.SetBool("playerOnSight", true);
        TextMeshProUGUI textContent = text.GetComponentInChildren<TextMeshProUGUI>();
        textContent.text = "!";
        textContent.color = new Color(255, 15, 0);
        textContent.outlineColor = new Color(255, 15, 0);
        text.SetActive(true);
    }

    public void PlayerLost()
    {
        _state = EnemyState.SEARCHING;
        _agent.speed = 0.7f;
        _agent.stoppingDistance = 0.2f;
    }

    IEnumerator shoot()
    {
        _canShot = false;
        burstFireSound.Play();
        muzzle.SetActive(true);
        _player.Damage(5.0f);
        yield return new WaitForSeconds(0.5f);
        muzzle.SetActive(false);
        yield return new WaitForSeconds(Random.Range(2.0f, 3.0f));
        _canShot = true;
    }
}
