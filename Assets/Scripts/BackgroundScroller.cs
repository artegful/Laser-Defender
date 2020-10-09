using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float _backgroundScrollSpeed = 0.5f;

    private Material _material;
    private Vector2 _offset;

    void Start()
    {
        _material = GetComponent<Renderer>().material;
        _offset = new Vector2(0, _backgroundScrollSpeed);
    }

    void Update()
    {
        _material.mainTextureOffset += _offset * Time.deltaTime;
    }
}
