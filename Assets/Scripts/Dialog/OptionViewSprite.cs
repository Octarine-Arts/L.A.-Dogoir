using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;
using TMPro;

public class OptionViewSprite : MonoBehaviour, IPointerDownHandler
{
    public List<Sprite> spriteList;
    public Color clickedColor;
    public Color textClickedColor;

    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();

        _image.sprite = spriteList[Random.Range(0, spriteList.Count)];
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        _image.color = clickedColor;
        transform.GetChild(0).GetComponent<TMP_Text>().color = textClickedColor;
    }
}
