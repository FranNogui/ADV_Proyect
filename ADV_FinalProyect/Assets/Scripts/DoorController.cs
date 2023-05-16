using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] MeshRenderer door;
    [SerializeField] Material transparentMaterial;
    [SerializeField] AudioSource doorSlide;
    public bool canBeOpened;
    int _playerIn;
    int _enemiesIn;

    private void Start()
    {
        _playerIn = 0;
        _enemiesIn = 0;
    }

    private void Update()
    {
        if (_playerIn + _enemiesIn > 0)
        {
            if (!animator.GetBool("character_nearby"))
            {
                animator.SetBool("character_nearby", true);
                doorSlide.Play();
            }
        }   
        else
        {
            if (animator.GetBool("character_nearby"))
            {
                animator.SetBool("character_nearby", false);
                doorSlide.Play();
            }
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && canBeOpened)
            _playerIn++;
        else if (other.tag == "Enemy")
            _enemiesIn++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && canBeOpened)
            _playerIn--;
        else if (other.tag == "Enemy" && animator.GetBool("character_nearby"))
            _enemiesIn--;
    }
}
