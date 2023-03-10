using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Impulse(float force)
    {
        _rb.AddForce(direction * force, ForceMode.Impulse);
    }
}
