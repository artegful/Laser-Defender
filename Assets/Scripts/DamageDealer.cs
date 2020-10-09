using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int _damage = 50;
    [SerializeField] float _particlesDuration = 0.1f;
    [SerializeField] GameObject _particlePrefab;
    public int Damage => _damage;

    public void Hit()
    {
        Destroy(gameObject);

        if (_particlePrefab != null)
        {
            Vector3 particlesPos = new Vector3(transform.position.x, transform.position.y, -1);
            GameObject particles = Instantiate(_particlePrefab, particlesPos, Quaternion.identity);
            Destroy(particles, _particlesDuration);
        }
    }
}
