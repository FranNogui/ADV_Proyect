using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallTrigger : MonoBehaviour
{
    [SerializeField] private Color myColor;
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private AudioSource sparkSound;
    [SerializeField] private TextMeshProUGUI scoreLabel;
    private int myScore;

    public int MyScore
    {
        get { return myScore; }
    }

    private void Start()
    {
        myColor = 32 * myColor;
        scoreLabel.color = Color.gray;
        myScore = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        BallColor bc = other.gameObject.GetComponent<BallColor>();

        if (bc != null)
        {
            bc.ChangeColor(myColor);
            foreach (ParticleSystem p in particles)
                p.Play();
            sparkSound.Play();
            scoreLabel.color = Color.white;
            myScore = Int32.Parse(scoreLabel.text);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BallColor bc = other.gameObject.GetComponent<BallColor>();

        if (bc != null)
        {
            scoreLabel.color = Color.gray;
            myScore = 0;
        }
    }
}
