using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _padding = 0.4f;
    [SerializeField] private int _health = 200;
    [SerializeField] private float _explosionDuration = 0.4f;
    [SerializeField] private int _invincibilityFlicks = 3;
    [SerializeField] private float _invincibilityFlickDuration = 0.5f;
    [SerializeField] private float _flyAwaySpeed = 10f;
    [SerializeField] private float _shakeMagnitude;
    [SerializeField] private float _shakeDuration;
    [SerializeField] private GameObject _explosionPrefab;

    [Header("Projectile")]
    [SerializeField] private float _projectileSpeed = 10f;
    [SerializeField] private float _fireCooldownDuration = 0.1f;
    [SerializeField] private float _laserOffset = 1f;
    [SerializeField] private GameObject _laserPrefab;

    [Header("SFX")]
    [SerializeField] [Range(0f, 1f)] private float _deathSoundVolume = 0.75f;
    [SerializeField] [Range(0f, 1f)] private float _shootSoundVolume = 0.75f;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _winSound;

    private Animator _animator;
    private PolygonCollider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private MenuController _menuController;
    private Transform _cameraTransform;
    private Vector3 _cameraInitialPos;

    private Coroutine _firingCoroutine;
    private float _xMin;
    private float _xMax;
    private float _yMin;
    private float _yMax;

    public EventHandler PlayerDied;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<PolygonCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _menuController = FindObjectOfType<MenuController>();
        _cameraTransform = Camera.main.transform;
        _cameraInitialPos = _cameraTransform.position;

        SetUpMovementBoundaries();
    }

    private void SetUpMovementBoundaries()
    {
        Camera gameCamera = Camera.main;
        _xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + _padding;
        _xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - _padding;
        _yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + _padding;
        _yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - _padding;
    }

    void Update()
    {
        Move();
        Fire();
    }

    private void Move()
    {
        float deltaXPos = Input.GetAxis("Horizontal") * Time.deltaTime * _moveSpeed;
        float deltaYPos = Input.GetAxis("Vertical") * Time.deltaTime * _moveSpeed;

        SetUpAnimationStates();

        Vector2 newPos = new Vector2
            (Mathf.Clamp(transform.position.x + deltaXPos, _xMin, _xMax),
             Mathf.Clamp(transform.position.y + deltaYPos, _yMin, _yMax));
        transform.position = newPos;
    }

    private void SetUpAnimationStates()
    {
        _animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(_firingCoroutine);
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            var laserPosition = new Vector2(transform.position.x, transform.position.y + _laserOffset);
            GameObject laser = Instantiate(
                _laserPrefab,
                laserPosition,
                Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, _projectileSpeed);
            AudioSource.PlayClipAtPoint(_shootSound, Camera.main.transform.position, _shootSoundVolume);

            yield return new WaitForSeconds(_fireCooldownDuration);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        if (damageDealer == null)
        {
            return;
        }
        _health -= damageDealer.Damage;
        ShakeScreen();

        damageDealer.Hit();
        _menuController.DecreaseHealth();

        if (_health <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(nameof(InvincibilityCoroutine));
    }

    private void ShakeScreen()
    {
        StartCoroutine(nameof(ShakeScreenCoroutine));
    }

    private IEnumerator ShakeScreenCoroutine()
    {
        float durationLeft = _shakeDuration;
        while (durationLeft > 0)
        {
            _cameraTransform.position =
                _cameraInitialPos + UnityEngine.Random.insideUnitSphere * _shakeMagnitude;
            durationLeft -= Time.deltaTime;

            yield return null;
        }

        _cameraTransform.position = _cameraInitialPos;
    }

    private IEnumerator InvincibilityCoroutine()
    {
        _collider.enabled = false;

        for(int i = 0; i < _invincibilityFlicks; i++)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;

            yield return new WaitForSeconds(_invincibilityFlickDuration);
        }
        _collider.enabled = true;
    }

    private void Die()
    {
        PlayerDied?.Invoke(this, new EventArgs());
        StopAllCoroutines();

        GameObject explosion = Instantiate(_explosionPrefab, transform.position, transform.rotation);
        Destroy(explosion, _explosionDuration);
        AudioSource.PlayClipAtPoint(_deathSound, Camera.main.transform.position, _deathSoundVolume);

        Destroy(gameObject);
    }

    public void FlyAway()
    {
        _yMax *= 2;
        StartCoroutine(nameof(FlyAwayCoroutine));
    }

    private IEnumerator FlyAwayCoroutine()
    {
        FindObjectOfType<MusicController>().StopMainTheme();
        yield return new WaitForSeconds(1f);

        AudioSource.PlayClipAtPoint(_winSound, transform.position, 2);
        while (transform.position.y < _yMax)
        {
            transform.position += Vector3.up * Time.deltaTime * _flyAwaySpeed;
            yield return new WaitForEndOfFrame();
        }

        Die();
    }
}
