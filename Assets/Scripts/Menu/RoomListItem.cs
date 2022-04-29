using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class RoomListItem : MonoBehaviour, IPointerEnterHandler
{
    public TMP_Text roonNameText;

    public AudioClip hoverSound;
    public AudioClip clickSound;
    
    private RoomInfo _info;
    
    public void SetUpRoom(RoomInfo info)
    {
        _info = info;
        roonNameText.text = info.Name;
    }

    public void OnClick()
    {
        AudioManager.current.PlaySFX(clickSound);
        Launcher.current.JoinRoom(_info);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.current.PlaySFX(hoverSound);
    }
}
