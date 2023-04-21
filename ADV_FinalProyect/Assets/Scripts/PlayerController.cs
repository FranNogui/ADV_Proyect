using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkVelocity;
    float _velocity;
    CharacterController _character;
    Animator _animator;

    public void Start()
    {
        _character = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _velocity = walkVelocity;
    }

    private void Update()
    {
        Camera camera = Camera.main;
        Vector3 camForward = camera.transform.forward;
        Vector3 camRight = camera.transform.right;
        camForward.y = 0.0f;
        camRight.y = 0.0f;
        camForward.Normalize();
        camRight.Normalize();
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 movement = camForward * y + camRight * x;
        _animator.SetFloat("x", Mathf.Lerp(_animator.GetFloat("x"), movement.x, Time.deltaTime * 7.0f));
        _animator.SetFloat("y", Mathf.Lerp(_animator.GetFloat("y"), movement.z, Time.deltaTime * 7.0f));
        if (movement.magnitude > 1) movement.Normalize();
        movement *= _velocity;
        movement = transform.TransformDirection(movement);
        _character.Move(movement * Time.deltaTime);
        Quaternion targetRot = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10.0f * Time.deltaTime);
    }
}
