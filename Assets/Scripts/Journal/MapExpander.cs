using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapExpander : MonoBehaviour
{
    public RectTransform expandTarget;

    private Button _mapButton;
    private Vector3 _startPosition;
    private bool _isExpanded;
    
    private void Awake()
    {
        _mapButton = GetComponent<Button>();
        _startPosition = transform.position;
        
        _mapButton.onClick.AddListener(ExpandMap);
    }

    private void ExpandMap()
    {
        if (_isExpanded) return;
        StopAllCoroutines();
        StartCoroutine(ExpandMap_CO());
    }

    public void ShrinkMap()
    {
        if (!_isExpanded) return;
        StopAllCoroutines();
        StartCoroutine(ShrinkMap_CO());
    }

    private IEnumerator ExpandMap_CO()
    {
        _isExpanded = true;
        float startTime = Time.time;
        Vector3 initialPosition = transform.position;

        while (startTime < Time.time + 0.5f)
        {
            transform.localScale = Vector3.Lerp(new Vector3(0.5f,0.5f,0.5f), new Vector3(1f,1f,1f), (Time.time - startTime)/0.5f);
            transform.position = Vector3.Lerp(initialPosition, expandTarget.position, (Time.time - startTime)/0.5f);
            yield return null;
        }
    }
    
    private IEnumerator ShrinkMap_CO()
    {
        _isExpanded = false;
        float startTime = Time.time;
        Vector3 initialPosition = transform.position;
        
        while (startTime < Time.time + 0.5f)
        {
            transform.localScale = Vector3.Lerp(new Vector3(1f,1f,1f),new Vector3(0.5f,0.5f,0.5f), (Time.time - startTime)/0.5f);
            transform.position = Vector3.Lerp(initialPosition, _startPosition, (Time.time - startTime)/0.5f);
            yield return null;
        }
    }
}
