using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager current;

    public static PlayerSpecies ThisPlayer => thisPlayer;
    private static PlayerSpecies thisPlayer;

    PhotonView pview;

    private void Awake ()
    {
        current = this;
        pview = GetComponent<PhotonView> ();
    }

    private void Start ()
    {
        if (pview.IsMine)
            CreateController ();
    }

    public event Action onPlayersSpawned;
    private void CreateController ()
    {
        //FindObjectOfType<SpawnPoint> ();
        Hashtable h = PhotonNetwork.CurrentRoom.CustomProperties;

        int hostSlot = (int)h["HSlot"];
        int guestSlot = (int)h["GSlot"];

        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate (Path.Combine ("PhotonPrefabs", hostSlot == 1 ? "HumanPlayer" : "DogPlayer"), Vector3.zero, Quaternion.identity);
            thisPlayer = hostSlot == 1 ? PlayerSpecies.Human : PlayerSpecies.Dog;
        }
        else
        {
            PhotonNetwork.Instantiate (Path.Combine ("PhotonPrefabs", guestSlot == 1 ? "HumanPlayer" : "DogPlayer"), Vector3.zero, Quaternion.identity);
            thisPlayer = guestSlot == 1 ? PlayerSpecies.Human : PlayerSpecies.Dog;
        }

        onPlayersSpawned?.Invoke();
        print ("Instantiated biiitch " + PhotonNetwork.LocalPlayer.ActorNumber);
    }

    private int IdentifyPlayer (string name)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            if (PhotonNetwork.PlayerList[i].NickName == name)
                return i;

        return -1;
    }
}
