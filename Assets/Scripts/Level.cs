using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private float _transitionTime = 1.5f;

    private Player _player;
    private WaveSpawner _waveSpawner;
    private MenuController _menuController;
    private MusicController _musicController;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _waveSpawner = FindObjectOfType<WaveSpawner>();
        _menuController = FindObjectOfType<MenuController>();
        _musicController = FindObjectOfType<MusicController>();

        _player.PlayerDied += GameOver;

        _player.enabled = false;
        _waveSpawner.enabled = false;
    }

    public void StartGame()
    {
        _musicController.PlayMainTheme();

        StartCoroutine(nameof(StartGameCoroutine));
    }

    private IEnumerator StartGameCoroutine()
    {
        _menuController.HideStartingMenu();
        _player.enabled = true;

        yield return new WaitForSeconds(_transitionTime);

        _waveSpawner.enabled = true;
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(0);
    }

    private void GameOver(object sender, EventArgs eventArgs)
    {
        _musicController.PlayEndingTheme();
        _menuController.ShowEndingMenu();

        _waveSpawner.StopAllCoroutines();
        _waveSpawner.enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
