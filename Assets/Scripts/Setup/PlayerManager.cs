using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    PhotonView pview;

    private void Awake ()
    {
        pview = GetComponent<PhotonView> ();
    }

    private void Start ()
    {
        if (pview.IsMine)
            CreateController ();
    }

    private void CreateController ()
    {
        //FindObjectOfType<SpawnPoint> ();
        Hashtable h = PhotonNetwork.CurrentRoom.CustomProperties;

        int hostSlot = (int)h["HSlot"];
        int guestSlot = (int)h["GSlot"];

        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate (Path.Combine ("PhotonPrefabs", hostSlot == 1 ? "HumanPlayer" : "DogPlayer"), Vector3.zero, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate (Path.Combine ("PhotonPrefabs", guestSlot == 1 ? "HumanPlayer" : "DogPlayer"), Vector3.zero, Quaternion.identity);
        }

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
