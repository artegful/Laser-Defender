using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private float _animationForce = 10f;
    [SerializeField] private float _destroyTime = 2f;
    [SerializeField] private float _gameOverTimeStep = 0.1f;
    [SerializeField] private float _heartAnimationTimeStep = 0.1f;
    [SerializeField] private GameObject _startingText;
    [SerializeField] private GameObject _startingButton;
    [SerializeField] private GameObject _quitButton;
    [SerializeField] private GameObject _endingGameText;
    [SerializeField] private GameObject _endingOverText;
    [SerializeField] private GameObject _endingButton;
    [SerializeField] private Image[] _hearts;
    [SerializeField] private Sprite _emptyHeart;

    private int _heartsCount = 3;

    public void HideStartingMenu()
    {
        _startingText.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, _animationForce);
        _startingButton.GetComponent<Rigidbody2D>().velocity = new Vector2(_animationForce, 0f);
        _quitButton.GetComponent<Rigidbody2D>().velocity = new Vector2(-_animationForce, 0f);
        _startingButton.GetComponent<Button>().enabled = false;
        _quitButton.GetComponent<Button>().enabled = false;
        Destroy(_startingText, _destroyTime);
        Destroy(_startingButton, _destroyTime);
        Destroy(_quitButton, _destroyTime);
        StartCoroutine(nameof(ShowHeartsCoroutine));
    }

    private IEnumerator ShowHeartsCoroutine()
    {
        for(int i = 0; i < _heartsCount; i++)
        {
            _hearts[i].enabled = true;
            yield return new WaitForSeconds(_heartAnimationTimeStep);
        }
    }

    public void ShowEndingMenu()
    {
        StartCoroutine(nameof(ShowGameOverCoroutine));
    }

    private IEnumerator ShowGameOverCoroutine()
    {
        yield return new WaitForSeconds(_gameOverTimeStep);
        _endingGameText.SetActive(true);
        yield return new WaitForSeconds(_gameOverTimeStep);
        _endingOverText.SetActive(true);
        yield return new WaitForSeconds(_gameOverTimeStep * 2);
        _endingButton.SetActive(true);
    }

    public void DecreaseHealth()
    {
        _hearts[--_heartsCount].sprite = _emptyHeart;
    }
}
