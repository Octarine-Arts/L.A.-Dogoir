using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using WebSocketSharp;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher current;
    
    [Header("Create Room")]
    public TMP_InputField roomInputField;
    public TMP_Text roomDisplayName;
    public TMP_Text errorDisplay;

    [Header("Find Room")]
    public Transform roomListContent;
    public GameObject roomListItemPrefab;

    [Header("Room")]
    public TMP_Text player1Name;
    public TMP_Text player2Name;

    private int _playerSlotNumber;

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
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString("0000");
        MainMenu_Manager.current.OpenMenu("Title Menu");
    }
    #endregion
    
    #region Misc Functions
    private void SwapSlots()
    {
        Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
        
        int hostSlot = (int) hashtable["HSlot"];
        int guestSlot = (int) hashtable["GSlot"];

        if (hostSlot == 1)
        {
            hashtable["HSlot"] = 2;
            if (guestSlot != 0) hashtable["GSlot"] = 1;
        }
        else if (hostSlot == 2)
        {
            hashtable["HSlot"] = 1;
            if (guestSlot != 0) hashtable["GSlot"] = 2;
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
    }

    private void ShowNames()
    {
        if (!PhotonNetwork.InRoom) return;
        
        Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
        int hostSlot = (int) hashtable["HSlot"];

        if (hostSlot == 1)
        {
            player1Name.text = (string) hashtable["HName"];
            if (PhotonNetwork.PlayerList.Length > 1) player2Name.text = (string) hashtable["GName"];
            else player2Name.text = "";

        }
        else if (hostSlot == 2)
        {
            player2Name.text = (string) hashtable["HName"];
            if(PhotonNetwork.PlayerList.Length > 1) player1Name.text = (string) hashtable["GName"];
            else player1Name.text = "";
        }
    }

    private void AssignSlotValue()
    {
        Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
        
        if (PhotonNetwork.IsMasterClient)
        {
            hashtable["HSlot"] = 1;
            hashtable["HName"] = PhotonNetwork.MasterClient.NickName;
        }
        else if(!PhotonNetwork.IsMasterClient)
        {
            switch ((int) hashtable["HSlot"])
            {
                case 1:
                    hashtable["GSlot"] = 2;
                    break;
                case 2:
                    hashtable["GSlot"] = 1;
                    break;
            }
            hashtable["GName"] = PhotonNetwork.LocalPlayer.NickName;
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
    }

    private void ClearSlotValue()
    {
        if (PhotonNetwork.IsMasterClient) return;
        
        Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;

        hashtable["GSlot"] = 0;
        hashtable["GName"] = "";
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
    }
    #endregion
    
    #region Override Functions
    public override void OnJoinedRoom()
    {
        MainMenu_Manager.current.OpenMenu("Room Menu");
        roomDisplayName.text = PhotonNetwork.CurrentRoom.Name;

        AssignSlotValue();
    }
    
    public override void OnLeftRoom()
    {
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
            if (info.RemovedFromList) continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUpRoom(info);
        }
    }
    
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        ShowNames();
    }
    #endregion
    
    #region Create Room Functions
    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomInputField.text)) return;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        options.CustomRoomProperties = new Hashtable
        {
            {"HSlot", 0},
            {"HName", ""},
            {"GSlot", 0},
            {"GName", ""}
        };
        
        PhotonNetwork.CreateRoom(roomInputField.text, options);
        MainMenu_Manager.current.OpenMenu("Loading Page");
    }
    #endregion
    
    #region Find Room Functions
    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MainMenu_Manager.current.OpenMenu("Loading Page");
    }
    #endregion
    
    #region In Room Functions
    public void LeaveRoom()
    {
        ClearSlotValue();
        
        PhotonNetwork.LeaveRoom();
        MainMenu_Manager.current.OpenMenu("Loading Page");
    }
    
    public void SwapSlotClicked()
    {
        SwapSlots();
    }

    public void StartGameClicked()
    {
        PhotonNetwork.LoadLevel(2);
    }
    #endregion

    public void QuitGame()
    {
        Application.Quit();
    }
}
