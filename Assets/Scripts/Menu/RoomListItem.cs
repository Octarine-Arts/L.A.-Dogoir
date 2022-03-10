using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using TMPro;

public class RoomListItem : MonoBehaviour
{
    public TMP_Text roonNameText;

    private RoomInfo _info;
    
    public void SetUpRoom(RoomInfo info)
    {
        _info = info;
        roonNameText.text = info.Name;
    }

    public void OnClick()
    {
        Launcher.current.JoinRoom(_info);
    }
}
