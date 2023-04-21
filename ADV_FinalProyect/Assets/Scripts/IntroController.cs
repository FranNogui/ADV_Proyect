using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    [SerializeField] PlayableDirector director;
    [SerializeField] IntroTextController text;
    [SerializeField] CinemachineVirtualCamera vcam1;
    [SerializeField] CinemachineVirtualCamera vcam2;
    [SerializeField] CinemachineVirtualCamera vcam3;
    [SerializeField] ParticleSystem fog;
    [SerializeField] ParticleSystem snow;
    bool reproduced = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !reproduced)
        {
            reproduced = true;
            director.Play();
            vcam1.gameObject.SetActive(true);
            vcam2.gameObject.SetActive(false);
            fog.Play();
            snow.Play();
            StartCoroutine(TextReproducer());
        }
    }

    IEnumerator TextReproducer()
    {
        yield return new WaitForSeconds(12.0f);
        text.Reproduce();
        yield return new WaitForSeconds(4.0f);
        vcam1.gameObject.SetActive(false);
        vcam3.gameObject.SetActive(true);
        yield return new WaitForSeconds(16.0f);
        SceneManager.LoadScene("Scene1");
    }
}
