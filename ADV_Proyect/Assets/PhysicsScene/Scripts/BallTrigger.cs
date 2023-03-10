using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    private Color myColor;

    private void Start()
    {
        myColor = transform.parent.Find("ScoreFloor").GetComponent<MeshRenderer>().material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        BallColor bc = other.gameObject.GetComponent<BallColor>();

        if (bc != null)
        {
            bc.ChangeColor(myColor);
        }
    }
}
