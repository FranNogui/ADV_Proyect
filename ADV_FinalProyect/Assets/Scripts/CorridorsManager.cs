using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CorridorsDirection
{
    Nord,
    East,
    West,
    South,
    NordEast,
    NordWest,
    SouthEast,
    SouthWest
}

public class CorridorsManager : MonoBehaviour
{
    [SerializeField] Transform player;
    CorridorController[] _corridors;
    CorridorsDirection _direction;

    private void Start()
    {
        _corridors = FindObjectsOfType<CorridorController>();
        _direction = CorridorsDirection.Nord;
    }

    private void Update()
    {
        float actRotationY = player.rotation.eulerAngles.y;

        if (actRotationY >= 22.5f && actRotationY <= 67.5f)
        {
            if (_direction != CorridorsDirection.NordWest)
            {
                foreach (var corridor in _corridors)
                    corridor.SetNordWest();
                _direction = CorridorsDirection.NordWest;
            }
        }
        else if (actRotationY >= 67.5f && actRotationY <= 112.5f)
        {
            if (_direction != CorridorsDirection.West)
            {
                foreach (var corridor in _corridors)
                    corridor.SetWest();
                _direction = CorridorsDirection.West;
            }
        }
        else if (actRotationY >= 112.5f && actRotationY <= 157.5f)
        {
            if (_direction != CorridorsDirection.SouthWest)
            {
                foreach (var corridor in _corridors)
                    corridor.SetSouthWest();
                _direction = CorridorsDirection.SouthWest;
            }
        }
        else if (actRotationY >= 157.5f && actRotationY <= 202.5f)
        {
            if (_direction != CorridorsDirection.South)
            {
                foreach (var corridor in _corridors)
                    corridor.SetSouth();
                _direction = CorridorsDirection.South;
            }
        }
        else if (actRotationY >= 202.5f && actRotationY <= 247.5f)
        {
            if (_direction != CorridorsDirection.SouthEast)
            {
                foreach (var corridor in _corridors)
                    corridor.SetSouthEast();
                _direction = CorridorsDirection.SouthEast;
            }
        }
        else if (actRotationY >= 247.5f && actRotationY <= 292.5f)
        {
            if (_direction != CorridorsDirection.East)
            {
                foreach (var corridor in _corridors)
                    corridor.SetEast();
                _direction = CorridorsDirection.East;
            }
        }
        else if (actRotationY >= 292.5f && actRotationY <= 337.5f)
        {
            if (_direction != CorridorsDirection.NordEast)
            {
                foreach (var corridor in _corridors)
                    corridor.SetNordEast();
                _direction = CorridorsDirection.NordEast;
            }
        }
        else if (_direction != CorridorsDirection.Nord)
        {
            foreach (var corridor in _corridors)
                corridor.SetNord();
            _direction = CorridorsDirection.Nord;
        }
            
    }
}
