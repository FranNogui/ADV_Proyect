using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum TypeDoor
{
    YesPlayer,
    NoPlayer,
    RedDoor,
    GreenDoor,
    BlueDoor,
    ExitDoor
}

public class DoorController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] MeshRenderer door;
    [SerializeField] Material transparentMaterial;
    [SerializeField] AudioSource doorSlide;
    [SerializeField] TypeDoor type;
    [SerializeField] string nextLevel;
    int _playerIn;
    int _enemiesIn;

    private void Start()
    {
        _playerIn = 0;
        _enemiesIn = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U) && type == TypeDoor.ExitDoor)
        {
            SceneManager.LoadScene(nextLevel);
        }

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
        if (other.tag == "Player")
        {
            if (type == TypeDoor.ExitDoor)
            {
                SceneManager.LoadScene(nextLevel);
            }

            PlayerController pc = other.GetComponent<PlayerController>();
            if (type == TypeDoor.YesPlayer ||
                (type == TypeDoor.RedDoor && pc.keyRed) ||
                (type == TypeDoor.GreenDoor && pc.keyGreen) ||
                (type == TypeDoor.BlueDoor && pc.keyBlue))
            {
                _playerIn++;
            }
        }
            
        else if (other.tag == "Enemy")
            _enemiesIn++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && _playerIn > 0)
            _playerIn--;
        else if (other.tag == "Enemy" && animator.GetBool("character_nearby"))
            _enemiesIn--;
    }
}
