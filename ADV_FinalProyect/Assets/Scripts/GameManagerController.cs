using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    [SerializeField] DoorOption[] doorClosedOptions;

    private void Start()
    {
        int option = Random.Range(0, doorClosedOptions.Length);
        foreach (var door in doorClosedOptions[option].doors)
            door.canBeOpened = false;
    }
}
