using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveIA : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject bandera;
    private GameObject banderaInstancia;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        banderaInstancia = Instantiate(bandera);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                agent.destination = banderaInstancia.transform.position = hit.point;
                banderaInstancia.GetComponent<AudioSource>().Play();
            }
        }
    }
}
