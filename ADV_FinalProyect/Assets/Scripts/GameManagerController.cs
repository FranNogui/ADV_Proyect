using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    [SerializeField] GameObject text;
    [SerializeField] GameObject instructions;

    private void Start()
    {
        StartCoroutine(QuitText());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            text.SetActive(false);
            instructions.SetActive(!instructions.activeSelf);
        }
    }

    IEnumerator QuitText()
    {
        yield return new WaitForSeconds(3.0f);
        text.SetActive(true);
    }
}
