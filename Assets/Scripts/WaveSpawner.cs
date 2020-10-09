using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private bool _looping = false;
    [SerializeField] private List<WaveConfig> _waveConfigs;

    private int _startingWave = 0;

    private IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        } while (_looping);
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = _startingWave; waveIndex < _waveConfigs.Count; waveIndex++)
        {
            WaveConfig currentWave = _waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            yield return new WaitForSeconds(currentWave.WaveCooldown);
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig wave)
    {
        for(int i = 0; i < wave.NumberOfEnemies; i++)
        {
            var newEnemy = Instantiate
                (wave.EnemyPrefab, wave.Waypoints[0].position, 
                Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(wave);

            yield return new WaitForSeconds(wave.TimeBetweenSpawns);
        }
    }
}
