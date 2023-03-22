using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class BallGameController : MonoBehaviour
{
    [SerializeField] private GameObject _ball;
    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;
    [SerializeField] private float accForce;
    [SerializeField] private GameObject bar;
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private BallTrigger[] scoreFloors;
    [SerializeField] private Transform ballStartPosition;
    [SerializeField] private PlayableDirector[] speakers;
    [SerializeField] private PlayableDirector crowd;
    [SerializeField] private PlayableDirector successSound;
    [SerializeField] private ConfettiScript[] confeti;
    [SerializeField] private PlayableDirector lights;
    [SerializeField] private GameObject freeLookCamera1;
    private int _score;
    private bool _canImpulse;
    private bool _addForce;
    private float _force;

    private int actualScore
    {
        get
        {
            int score = 0;
            foreach (BallTrigger tr in scoreFloors)
                score = Mathf.Max(score, tr.MyScore);
            return score;
        }
    }

    private void Start()
    {
        _force = 0.0f;
        _addForce = true;
        _canImpulse = true;
        _score = 0;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.Space) && _canImpulse)
        {
            if (_addForce) _force += accForce * Time.deltaTime;
            else _force -= accForce * Time.deltaTime;

            if (_force > maxForce)
            {
                _force = maxForce;
                _addForce = false;
            }
            else if (_force < minForce)
            {
                _force = minForce;
                _addForce = true;
            }
            if (bar != null)
                bar.transform.localScale = new Vector3(1.0f, _force / maxForce, 1.0f);
        }

        if (Input.GetKeyUp(KeyCode.Space) && _canImpulse)
        {
            _ball.GetComponent<BallMovement>().Impulse(_force);
            _canImpulse = false;
            StartCoroutine(RestartBall());
            freeLookCamera1.SetActive(false);
        }
    }

    IEnumerator RestartBall()
    {
        yield return new WaitForSeconds(0.1f);
        while (true)
        {
            if (Mathf.Approximately(_ball.GetComponent<Rigidbody>().velocity.magnitude, 0.0f))
            {
                _score += actualScore;
                if (actualScore > 80)
                {
                    lights.Play();
                    foreach (PlayableDirector p in speakers)
                        p.Play();
                    foreach (ConfettiScript c in confeti)
                        c.EmiteConfetti();
                }
                else if (actualScore <= 0)
                    crowd.Play();
                else
                    successSound.Play();
                scoreLabel.text = "SCORE\n" + _score;
                freeLookCamera1.SetActive(true);
                yield return new WaitForSeconds(2);
                _ball.transform.position = ballStartPosition.position;
                _canImpulse = true;
                _force = 0.0f;
                if (bar != null)
                    bar.transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
                break;
            }
            yield return 0;
        }
    }

}
