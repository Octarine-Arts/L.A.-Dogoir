using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    public AudioClip hoverSound;
    public AudioClip clickSound;

    public Image image;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image != null) image.enabled = true;
        AudioManager.current.PlaySFX(hoverSound);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (image != null) image.enabled = false;
        AudioManager.current.PlaySFX(clickSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (image != null) image.enabled = false;
    }
}
