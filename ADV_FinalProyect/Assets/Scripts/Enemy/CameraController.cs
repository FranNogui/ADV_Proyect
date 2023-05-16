using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.AI;

public class CameraController : MonoBehaviour
{
    [SerializeField] SightTrigger sight;
    [SerializeField] Light sightLight;
    [SerializeField] float alertnessFactor;
    [SerializeField] float alertnessLossFactor;
    [SerializeField] Transform[] lookPositions;
    [SerializeField] float velocity;
    [SerializeField] float waitTime;
    bool _waiting;
    int _currentPos;
    EnemyManager _manager;
    float _alertness;

    private void Start()
    {
        _alertness = 0.0f;
        _manager = FindObjectOfType<EnemyManager>();
        _currentPos = 0;
        _waiting = false;
        StartCoroutine(Wait());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.rotation.eulerAngles);

        if (!_waiting)
            StartCoroutine(Wait());

        var lookPos = lookPositions[_currentPos].position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * velocity);

        if (CheckPlayerInSight())
        {
            Vector3 playerPos = sight.Player.transform.position;
            float distance = Mathf.Abs((this.transform.position - playerPos).magnitude);
            _alertness += alertnessFactor * (1 / (distance * distance)) * Time.deltaTime;
        }
        else
        {
            _alertness -= alertnessLossFactor * Time.deltaTime;
        }

        _alertness = Mathf.Clamp(_alertness, 0.0f, 100.0f);
        sightLight.color = Color.Lerp(Color.green, Color.red, _alertness / 100.0f);

        if (Mathf.Approximately(_alertness, 100.0f))
        {
            _manager.LastPlayerPosition = sight.Player.transform.position;
            _manager.PlayerFound();
        }
    }

    bool CheckPlayerInSight()
    {
        if (sight.CanSeePlayer)
        {
            Vector3 start = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Vector3 end = sight.Player.transform.position;
            return !Physics.Linecast(start, end);
        }

        return false;
    }

    IEnumerator Wait()
    {
        _waiting = true;
        yield return new WaitForSeconds(waitTime);
        _waiting = false;
        _currentPos = (_currentPos + 1) % lookPositions.Length;
    }
}
