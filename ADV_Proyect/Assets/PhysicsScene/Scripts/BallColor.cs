using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColor : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ChangeColor(Color newColor)
    {
        _meshRenderer.material.SetColor("lightColor", newColor);
    }

    public void ChangeIntensity(float newIntensity)
    {
        _meshRenderer.material.SetFloat("intensity", newIntensity);
    }
}
