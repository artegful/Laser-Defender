using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyShootPattern : ShootPattern
{
    [SerializeField] protected int _numberOfDiretions;
    [SerializeField] protected int _numberOfWaves;
    [SerializeField] protected float _timeBetweenWaves;
    [SerializeField] protected float _fullAngle;

    protected override IEnumerator ShootCoroutine()
    {
        while (true)
        {
            Vector2 projectilePos = transform.position;
            projectilePos.y -= _projectileYOffset;

            if (_playerTransform == null)
            {
                break;
            }
            Vector3 direction = _playerTransform.transform.position - transform.position;
            AudioSource.PlayClipAtPoint(_shootSound, Camera.main.transform.position, _shootSoundVolume);
            direction.Normalize();

            for(int i = 0; i < _numberOfWaves; i++)
            {
                for (int j = -_numberOfDiretions / 2; j <= _numberOfDiretions / 2; j++)
                {
                    GameObject projectile = Instantiate
                    (_projectilePrefab, projectilePos, Quaternion.identity)
                    as GameObject;

                    projectile.GetComponent<Rigidbody2D>().velocity
                        = Quaternion.Euler(0, 0, _fullAngle / _numberOfDiretions * j) * direction * _projectileSpeed;
                }

                yield return new WaitForSeconds(_timeBetweenWaves);
            }

            yield return new WaitForSeconds
               (UnityEngine.Random.Range(_minTimeBetweenShots, _maxTimeBetweenShots));
        }
        
    }
}
