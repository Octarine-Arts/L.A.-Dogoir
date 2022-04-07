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
        GameObject spawnedPrefab;

        int hostSlot = (int)h["HSlot"];
        int guestSlot = (int)h["GSlot"];

        if(PhotonNetwork.IsMasterClient)
        {
            thisPlayer = hostSlot == 1 ? PlayerSpecies.Human : PlayerSpecies.Dog;
            Vector3 spawnPosition = ThisPlayer == PlayerSpecies.Human ? GameObject.Find("HumanSpawnPoint").transform.position : GameObject.Find("DogSpawnPoint").transform.position;
            spawnedPrefab = PhotonNetwork.Instantiate (Path.Combine ("PhotonPrefabs", hostSlot == 1 ? "HumanPlayer" : "DogPlayer"), spawnPosition, Quaternion.identity);
        }
        else
        {
            thisPlayer = guestSlot == 1 ? PlayerSpecies.Human : PlayerSpecies.Dog;
            Vector3 spawnPosition = ThisPlayer == PlayerSpecies.Human ? GameObject.Find("HumanSpawnPoint").transform.position : GameObject.Find("DogSpawnPoint").transform.position;
            spawnedPrefab = PhotonNetwork.Instantiate (Path.Combine ("PhotonPrefabs", guestSlot == 1 ? "HumanPlayer" : "DogPlayer"), spawnPosition, Quaternion.identity);
        }

        onPlayersSpawned?.Invoke();
        //print ("Instantiated biiitch " + PhotonNetwork.LocalPlayer.ActorNumber);
    }

    private int IdentifyPlayer (string name)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            if (PhotonNetwork.PlayerList[i].NickName == name)
                return i;

        return -1;
    }
}
