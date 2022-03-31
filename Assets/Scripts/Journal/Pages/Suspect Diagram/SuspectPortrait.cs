using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SuspectPortrait : MonoBehaviour, IPointerEnterHandler
{
    public Suspect suspectSO;

    public Image suspectImage;
    public TMP_Text locationText;
    public TMP_Text colorText;
    
    public void Start()
    {
        suspectImage.sprite = suspectSO.displayImage;
        locationText.text = suspectSO.correctLocation.displayName;
        //colorText.text = suspectSO.color.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new NotImplementedException();
    }
}
