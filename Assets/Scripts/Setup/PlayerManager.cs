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

    public GameObject HumanPlayer { get; private set; }
    public GameObject DogPlayer { get; private set; }
    public bool PlayersSpawned { get; private set; }

    PhotonView pview;

    private void Awake ()
    {
        current = this;
        PlayersSpawned = false;
        pview = GetComponent<PhotonView> ();
    }

    private void Start ()
    {
        if (pview.IsMine)
            CreateController ();

        if (EventManager.I != null)
            EventManager.I.OnPlayersSpawned += OnPlayersSpawned;
    }

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

        //print ("Instantiated biiitch " + PhotonNetwork.LocalPlayer.ActorNumber);
    }

    private void OnPlayersSpawned (GameObject human, GameObject dog)
    {
        HumanPlayer = human;
        DogPlayer = dog;
        PlayersSpawned = true;
    }

    private int IdentifyPlayer (string name)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            if (PhotonNetwork.PlayerList[i].NickName == name)
                return i;

        return -1;
    }
}
