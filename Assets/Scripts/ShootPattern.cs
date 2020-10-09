using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPattern : MonoBehaviour
{
    [SerializeField] protected AudioClip _shootSound;
    [SerializeField] [Range(0f, 1f)] protected float _shootSoundVolume;
    [SerializeField] protected float _projectileSpeed;
    [SerializeField] protected float _projectileYOffset = 1f;
    [SerializeField] protected float _minTimeBetweenShots = 0.1f;
    [SerializeField] protected float _maxTimeBetweenShots = 0.5f;
    [SerializeField] protected GameObject _projectilePrefab;

    protected Transform _playerTransform;

    protected void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(nameof(ShootCoroutine));
    }

    protected void StopAll(object sender, EventArgs eventArgs)
    {
        StopCoroutine(nameof(ShootCoroutine));
    }

    protected virtual IEnumerator ShootCoroutine()
    {
        while (true)
        {
            Vector2 projectilePos = transform.position;
            projectilePos.y -= _projectileYOffset;

            AudioSource.PlayClipAtPoint(_shootSound, Camera.main.transform.position, _shootSoundVolume);
            if (_playerTransform == null)
            {
                break;
            }
            Vector2 direction = _playerTransform.position - transform.position;
            direction.Normalize();
            direction *= _projectileSpeed;

            GameObject projectile = Instantiate
                (_projectilePrefab, projectilePos, Quaternion.identity)
                as GameObject;

            projectile.GetComponent<Rigidbody2D>().velocity = direction;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            yield return new WaitForSeconds
               (UnityEngine.Random.Range(_minTimeBetweenShots, _maxTimeBetweenShots));
        }
    }
}