using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SpawnableObject : MonoBehaviour
{
    private readonly float _defaultTimerValue = -1f;

    private float _pushTimer;
    private LineRenderer _lineRenderer;
    private Spawner _spawner;
    private GameObject _gameObject;

    private void Update()
    {
        if (_pushTimer >= 0)
        {
            _pushTimer -= Time.deltaTime;

            if (_pushTimer < 0)
            {
                _pushTimer = _defaultTimerValue;
                Push();
            }
        }
    }

    public SpawnableObject Initialize(Spawner spawner)
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _spawner = spawner;
        _gameObject = gameObject;
        _gameObject.SetActive(false);
        _pushTimer = _defaultTimerValue;
        return this;
    }

    public void Push()
    {
        _spawner.Push(this);
    }

    public void PushDelayed(float time)
    {
        _pushTimer = time;
    }

    public SpawnableObject Pull(Vector3 start, Vector3 end)
    {
        SetActive(true);
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);

        return this;
    }

    public void SetActive(bool value)
    {
        _gameObject.SetActive(value);        
    }
}
