using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float velocity;
    [SerializeField] CinemachineFreeLook frontCamera;
    [SerializeField] CinemachineFreeLook leftCamera;
    [SerializeField] CinemachineFreeLook rightCamera;
    [SerializeField] CinemachineFreeLook deathCamera;
    [SerializeField] float maxHealth;
    [SerializeField] float healthTimer;
    [SerializeField] float healthAccel;
    [SerializeField] Image bloodBackground;
    [SerializeField] Image bloodEffect;
    [SerializeField] GameObject scene;
    [SerializeField] GameObject spotLight;
    [SerializeField] GameObject floor;
    [SerializeField] AudioSource noise;
    [SerializeField] AudioSource heartBeat;
    [SerializeField] AudioSource ouch;
    float _health;
    float _healthTimer;
    CharacterController _character;
    Animator _animator;

    public void Start()
    {
        _character = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _health = maxHealth;
        _healthTimer = 0.0f;
        deathCamera.Priority = 1;
    }

    private void Update()
    {
        if (_health <= 0.0f)
        {
            if (!noise.isPlaying)
            {
                noise.Play();
                ouch.Play();
                StartCoroutine(RestartLevel());
            }
            _animator.SetBool("Death", true);
            deathCamera.Priority = 10;
            frontCamera.Priority = 1;
            leftCamera.Priority = 1;
            rightCamera.Priority = 1;
            bloodBackground.color = new Color(0, 0, 0, 0);
            scene.SetActive(false);
            spotLight.SetActive(true);
            floor.SetActive(true);
            Physics.gravity = Vector3.zero;
            return;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            rightCamera.Priority = 1;
            frontCamera.Priority = 1;
            leftCamera.Priority = 10;
            _animator.SetFloat("x", Mathf.Lerp(_animator.GetFloat("x"), 0.0f, Time.deltaTime * 30.0f));
            _animator.SetFloat("y", Mathf.Lerp(_animator.GetFloat("y"), 0.0f, Time.deltaTime * 30.0f));
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rightCamera.Priority = 10;
            frontCamera.Priority = 1;
            leftCamera.Priority = 1;
            _animator.SetFloat("x", Mathf.Lerp(_animator.GetFloat("x"), 0.0f, Time.deltaTime * 30.0f));
            _animator.SetFloat("y", Mathf.Lerp(_animator.GetFloat("y"), 0.0f, Time.deltaTime * 30.0f));
        }
        else
        {
            rightCamera.Priority = 1;
            frontCamera.Priority = 10;
            leftCamera.Priority = 1;

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            transform.rotation = Quaternion.LerpUnclamped(transform.rotation,
                Quaternion.Euler(0.0f, Camera.main.transform.rotation.eulerAngles.y, 0.0f),
                Time.deltaTime * 5.0f);

            Vector3 movement = Vector3.forward * y + Vector3.right * x;

            _animator.SetFloat("x", Mathf.Lerp(_animator.GetFloat("x"), movement.x, Time.deltaTime * 10.0f));
            _animator.SetFloat("y", Mathf.Lerp(_animator.GetFloat("y"), movement.z, Time.deltaTime * 10.0f));

            if (movement.magnitude > 1) movement.Normalize();
            movement *= velocity;
            movement = transform.TransformDirection(movement);

            _character.Move(movement * Time.deltaTime);
        }

        if (_healthTimer < 0.0f && _health < maxHealth)
        {
            _health += healthAccel * Time.deltaTime;
            _health = Mathf.Clamp(_health, 0.0f, 100.0f);
        }
        else if (_healthTimer > 0.0f)
        {
            _healthTimer -= Time.deltaTime;
        }

        heartBeat.volume = _health / maxHealth;
        bloodBackground.color = new Color(bloodBackground.color.r, bloodBackground.color.g, bloodBackground.color.b, 0.5f * (1 - (_health / maxHealth)));
        bloodEffect.color = new Color(bloodEffect.color.r, bloodEffect.color.g, bloodEffect.color.b, (1 - (_health / maxHealth)));
    }

    public void Damage(float damage)
    {
        _healthTimer = healthTimer;
        _health -= damage;
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(7.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
