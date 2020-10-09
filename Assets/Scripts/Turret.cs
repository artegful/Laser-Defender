using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform _playerTransform;

    void Start()
    {
        _playerTransform = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        if (_playerTransform == null)
        {
            Destroy(this);
        }

        var targetPosition = _playerTransform.position; 

        Vector2 lookDirection = targetPosition - transform.position;
        if (lookDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
