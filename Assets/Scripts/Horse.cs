using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse
{
    private GameObject _view;
    private int _number;
    private float _speed;

    public GameObject View => _view;
    public int Number => _number;
    public float Speed => _speed;

    public Horse (GameObject view, int number, float speed)
    {
        _view = view;
        _number = number;
        _speed = speed;
    }

    public void SlowDown()
    {
        _speed = 0;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}