using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroTextController : MonoBehaviour
{
    [SerializeField] AudioSource[] clickSound;
    [SerializeField] char[] characters;
    [SerializeField] TextMeshProUGUI GUIText;
    [SerializeField] float delay;
    int audioIndex;
    int index;
   
    void Start()
    {
        audioIndex = 0;
        index = 0;
        GUIText.text = "";
    }

    public void Reproduce()
    {
        StartCoroutine(ReproduceCoroutine());
    }

    IEnumerator ReproduceCoroutine()
    {
        GUIText.text = "";
        audioIndex = 0;
        index = 0;
        while (index < characters.Length)
        {
            GUIText.text += characters[index];
            clickSound[audioIndex].Play();
            index++;
            audioIndex++;
            while (index < characters.Length && (characters[index] == ' ' || characters[index] == '\n'))
            {
                GUIText.text += characters[index];
                index++;
            }
            audioIndex %= clickSound.Length;
            yield return new WaitForSeconds(delay);
        }
        index = 0;
    }
}
