using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _pathPrefab;
    [SerializeField] private float _timeBetweenSpawns = 0.5f;
    [SerializeField] private float _randomFactor = 0.3f;
    [SerializeField] private int _numberOfEnemies = 5;
    [SerializeField] private float _movementSpeed = 2f;
    [SerializeField] private float _waveCooldownTime = 1f;
    [SerializeField] private bool _isDestroyedAtEnd = true;

    public GameObject EnemyPrefab => _enemyPrefab;
    public List<Transform> Waypoints
    {
        get
        {
            var waveWaypoints = new List<Transform>();

            foreach (Transform child in _pathPrefab.transform)
            {
                waveWaypoints.Add(child);
            }

            return waveWaypoints;
        }
    }
    public float TimeBetweenSpawns => _timeBetweenSpawns;
    public float RandomFactor => _randomFactor;
    public int NumberOfEnemies => _numberOfEnemies;
    public float MovementSpeed => _movementSpeed;
    public float WaveCooldown => _waveCooldownTime;
    public bool IsDestroyedAtEnd => _isDestroyedAtEnd;
}
