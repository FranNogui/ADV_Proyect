using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGameController : MonoBehaviour
{
    [SerializeField] private GameObject _ball;
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;
    [SerializeField] private float accForce;
    private bool _canImpulse;
    private bool _addForce;
    private float _force;

    private void Start()
    {
        _force = 0.0f;
        _addForce = true;
        _canImpulse = true;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _canImpulse)
        {
            if (_addForce) _force += accForce * Time.deltaTime;
            else _force -= accForce * Time.deltaTime;

            if (_force > maxForce)
            {
                _force = maxForce;
                _addForce = false;
            }
            else if (_force < minForce)
            {
                _force = minForce;
                _addForce = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && _canImpulse)
        {
            _ball.GetComponent<BallMovement>().Impulse(_force);
            _canImpulse = false;
        }
    }

}
