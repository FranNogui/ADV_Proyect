using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Rigidbody rigidBody;
    private float MAX_VEL_LIGHT = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKey(KeyCode.Space))
            GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 0.0f, 1.0f));

        meshRenderer.material.SetFloat("_Float", rigidBody.velocity.magnitude / MAX_VEL_LIGHT);

        if (transform.position.y < -10.0f)
        {
            transform.position = new Vector3(0.0f, 2.0f, -1.5f);
            rigidBody.Sleep();
        }
    }
}
