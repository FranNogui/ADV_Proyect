using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightTrigger : MonoBehaviour
{
    public bool CanSeePlayer;
    public GameObject Player;

    private void Start()
    {
        CanSeePlayer = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CanSeePlayer = true;
            Player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            CanSeePlayer = false;
    }
}
