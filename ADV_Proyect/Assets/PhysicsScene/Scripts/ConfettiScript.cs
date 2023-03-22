using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem confetti;
    [SerializeField] private AudioSource soundEmitter;

    public void EmiteConfetti()
    {
        confetti.Play();
        soundEmitter.Play();
    }
}
