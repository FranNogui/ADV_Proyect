using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Material wallEmission;
    [SerializeField, ColorUsageAttribute(true, true)] Color greenLight;
    [SerializeField, ColorUsageAttribute(true, true)] Color redLight;
    [SerializeField, ColorUsageAttribute(true, true)] Color redLight1;
    [SerializeField] AudioSource detectedSound;
    [SerializeField] AudioSource noDetectedSound;
    [SerializeField] AudioSource alarm;
    int _redLightValue;
    bool _playerFound;
    List<EnemyController> _enemies;
    Vector3 _lastPlayerPosition;

    private void Awake()
    {
        _enemies = new List<EnemyController>();
        _lastPlayerPosition = Vector3.zero;
        _redLightValue = 0;
        _playerFound = false;
    }

    public void Update()
    {
        if (_playerFound)
        {
            if (_redLightValue == 0 && wallEmission.GetColor("_EmissionColor") != redLight)
            {
                wallEmission.SetColor("_EmissionColor", Color.Lerp(wallEmission.GetColor("_EmissionColor"), redLight, 5.0f * Time.deltaTime));
                if (wallEmission.GetColor("_EmissionColor") == redLight)
                    _redLightValue = 1;
            }
            else if (_redLightValue == 1 && wallEmission.GetColor("_EmissionColor") != redLight1)
            {
                wallEmission.SetColor("_EmissionColor", Color.Lerp(wallEmission.GetColor("_EmissionColor"), redLight1, 5.0f * Time.deltaTime));
                if (wallEmission.GetColor("_EmissionColor") == redLight1)
                    _redLightValue = 0;
            }
        }
        else
        {
            if (wallEmission.GetColor("_EmissionColor") != greenLight)
                wallEmission.SetColor("_EmissionColor", Color.Lerp(wallEmission.GetColor("_EmissionColor"), greenLight, 5.0f * Time.deltaTime));
        }
    }

    public Vector3 LastPlayerPosition
    {
        get { return _lastPlayerPosition; }
        set { _lastPlayerPosition = value; }
    }

    public void AddEnemy(EnemyController newEnemy)
    {
        _enemies.Add(newEnemy);
    }

    public void PlayerFound()
    {
        foreach (var enemy in _enemies)
            enemy.PlayerFound();
        _playerFound = true;
        noDetectedSound.Stop();
        if (!detectedSound.isPlaying)
        {
            detectedSound.Play();
            alarm.Play();
        }   
    }

    public void PlayerLost()
    {
        foreach (var enemy in _enemies)
            enemy.PlayerLost();
        _playerFound = false;
        noDetectedSound.Play();
        detectedSound.Stop();
        alarm.Stop();
    }
}
