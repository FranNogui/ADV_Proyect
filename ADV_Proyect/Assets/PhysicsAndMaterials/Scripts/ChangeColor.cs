using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Material mat = this.gameObject.GetComponent<Renderer>().material;
        other.gameObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(mat.color.r, mat.color.g, mat.color.b, 255.0f));
        Debug.Log("Detected");
    }
}
