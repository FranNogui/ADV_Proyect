using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ColorKey
{
    RED,
    GREEN,
    BLUE
}

public class KeyController : MonoBehaviour
{
    [SerializeField] ColorKey colorKey;
    [SerializeField] GameObject[] lights;
    [SerializeField] AudioSource pickAudio;
    PlayerController _player;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (colorKey)
            {
                case ColorKey.RED:
                    _player.keyRed = true;
                    break;
                case ColorKey.GREEN:
                    _player.keyGreen = true;
                    break;
                case ColorKey.BLUE:
                    _player.keyBlue = true;
                    break;
            }
            foreach (var light in lights)
                light.SetActive(false);
            Destroy(GetComponent<MeshRenderer>());
            pickAudio.Play();
            StartCoroutine(WaitAndDestroy());
        }
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
