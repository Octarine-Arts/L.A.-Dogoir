using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SuspectPortrait : MonoBehaviour
{
    public BarSuspect_SO suspectSO;

    public Image suspectImage;
    public TMP_Text locationText;
    public TMP_Text colorText;
    
    public void Start()
    {
        suspectImage.sprite = suspectSO.image;
        locationText.text = suspectSO.correctLocation.locationName;
        colorText.text = suspectSO.color.ToString();
    }
}
