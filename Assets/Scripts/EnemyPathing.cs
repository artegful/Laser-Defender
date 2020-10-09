using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    private float _movementSpeed;
    private WaveConfig _waveConfig;
    private List<Transform> _waypoints;
    private bool _isDestroyedAtEnd;

    private int _waypointIndex = 0;

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        _waveConfig = waveConfig;
        _isDestroyedAtEnd = _waveConfig.IsDestroyedAtEnd;
    }

    private void Start()
    {
        _movementSpeed = _waveConfig.MovementSpeed;
        _waypoints = _waveConfig.Waypoints;
        transform.position = _waypoints[_waypointIndex].position;
        _waypointIndex++;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (_waypointIndex < _waypoints.Count)
        {
            var targetPosition = _waypoints[_waypointIndex].position;
            float movementThisFrame = Time.deltaTime * _movementSpeed;

            transform.position = Vector2.MoveTowards
                (transform.position, targetPosition, movementThisFrame);

            Vector2 moveDirection = targetPosition - transform.position;
            if (moveDirection != Vector2.zero)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg + 90;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            if (transform.position == targetPosition)
            {
                _waypointIndex++;
            }
        }
        else
        {
            if (_isDestroyedAtEnd)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(this);
            }
        }
    }
}

