using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OptionViewSprite : MonoBehaviour
{
    public List<Sprite> spriteList;
    
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();

        _image.sprite = spriteList[Random.Range(0, spriteList.Count)];
    }
}
