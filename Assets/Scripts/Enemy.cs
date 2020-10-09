using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _shotCounter;
    [SerializeField] private float _minTimeBetweenShots = 0.2f;
    [SerializeField] private float _maxTimeBetweenShots = 3f;
    [SerializeField] private float _explosionDuration = 0.3f;
    [SerializeField] private float _flickDuration = 0.05f;
    [SerializeField] private Color _shadedColor;
    [SerializeField] private GameObject _explosionPrefab;

    [Header("SFX")]
    [SerializeField] [Range(0f, 1f)] private float _deathSoundVolume = 0.75f;
    [SerializeField] private AudioClip _deathSound;

    private SpriteRenderer _spriteRenderer;

    private bool _animationCoroutineIsRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        _shotCounter = Random.Range(_minTimeBetweenShots, _maxTimeBetweenShots);
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        if(damageDealer == null)
        {
            return;
        }

        _health -= damageDealer.Damage;
        damageDealer.Hit();

        if (_health <= 0)
        {
            if (CompareTag("Finish"))
            {
                FindObjectOfType<Player>().FlyAway();
            }
           
            Destroy(gameObject);
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, transform.rotation);
            Destroy(explosion, _explosionDuration);
            AudioSource.PlayClipAtPoint(_deathSound, Camera.main.transform.position, _deathSoundVolume);
        }
        if (!_animationCoroutineIsRunning)
        {
            StartCoroutine(nameof(FlickCoroutine));
        }
    }

    private IEnumerator FlickCoroutine()
    {
        _animationCoroutineIsRunning = true;
        Color normalColor = _spriteRenderer.color;
        _spriteRenderer.color = _shadedColor;
        yield return new WaitForSeconds(_flickDuration);
        _spriteRenderer.color = normalColor;
        _animationCoroutineIsRunning = false;
    }
}
