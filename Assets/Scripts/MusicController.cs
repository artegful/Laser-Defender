using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private float _transitionGap;
    [SerializeField] private AudioClip _menuTheme;
    [SerializeField] private AudioClip _transitionTheme;
    [SerializeField] private AudioClip _mainTheme;

    private AudioSource _audioPlayer;

    private void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _audioPlayer = GetComponent<AudioSource>();
    }

    public void PlayMainTheme()
    {
        StartCoroutine(nameof(StartMainMusicCoroutine));
    }

    private IEnumerator StartMainMusicCoroutine()
    {
        _audioPlayer.Stop();
        _audioPlayer.clip = _mainTheme;
        _audioPlayer.PlayOneShot(_transitionTheme);
        yield return new WaitForSeconds(_transitionTheme.length - _transitionGap);

        _audioPlayer.Play();
    }

    public void StopMainTheme()
    {
        _audioPlayer.Stop();
    }

    public void PlayEndingTheme()
    {
        _audioPlayer.clip = _menuTheme;
        _audioPlayer.Play();
    }

}
