using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using WebSocketSharp;
using Random = UnityEngine.Random;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher current;
    
    public TMP_InputField roomInputField;
    public TMP_Text roomDisplayName;
    public TMP_Text errorDisplay;

    public Transform roomListContent;
    public GameObject roomListItemPrefab;

    public TMP_Text player1Name;
    public TMP_Text player2Name;

    private int _playerSlotNumber;

    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();
    
    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        MainMenu_Manager.current.OpenMenu("Loading Page");
    }

    #region Connect To Server
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        MainMenu_Manager.current.OpenMenu("Title Menu");
    }
    #endregion
    
    #region Misc Functions

    private void ShowNames()
    {
        
    }

    private void RemoveName()
    {
        if (_playerSlotNumber == 1)
        {
            player1Name.text = "";
        }
        else if (_playerSlotNumber == 2)
        {
            player2Name.text = "";
        }

        SetSlotValue();
    }

    private void SetSlotValue()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _myCustomProperties["HostSlotValue"] = _playerSlotNumber;
            _myCustomProperties["HostName"] = PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {
            switch ((int) _myCustomProperties["HostSlotValue"])
            {
                case 1:
                    _myCustomProperties["GuestSlotValue"] = 2;
                    break;
                case 2:
                    _myCustomProperties["GuestSlotValue"] = 1;
                    break;
            }
            _myCustomProperties["GuestName"] = PhotonNetwork.LocalPlayer.NickName;
        }
        
        PhotonNetwork.LocalPlayer.CustomProperties = _myCustomProperties;
    }
    #endregion
    
    #region Room Functions
    public override void OnJoinedRoom()
    {
        MainMenu_Manager.current.OpenMenu("Room Menu");
        roomDisplayName.text = PhotonNetwork.CurrentRoom.Name;

        SetSlotValue();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }

    public override void OnLeftRoom()
    {
        RemoveName();
        MainMenu_Manager.current.OpenMenu("Title Menu");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorDisplay.text = "Room Creation Failed: " + message;
        MainMenu_Manager.current.OpenMenu("Error Page");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        
        foreach (RoomInfo info in roomList)
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUpRoom(info);
        }
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomInputField.text)) return;

        PhotonNetwork.CreateRoom(roomInputField.text);
        MainMenu_Manager.current.OpenMenu("Loading Page");
    }
    
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MainMenu_Manager.current.OpenMenu("Loading Page");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MainMenu_Manager.current.OpenMenu("Loading Page");
    }
    #endregion
    
    public void QuitGame()
    {
        Application.Quit();
    }

    [PunRPC]
    private int RPC_GetSlotValue()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (_playerSlotNumber == 1) return 2;
            return 1;
        }

        return 0;
    }
}
