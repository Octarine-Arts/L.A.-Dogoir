using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

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
        //PhotonNetwork.Instantiate (Path.Combine ("PhotonPrefabs", IdentifyPlayer() == 0 ? "HumanPlayer" : "DogPlayer"), Vector3.zero, Quaternion.identity);
        print ("Instantiated biiitch " + PhotonNetwork.LocalPlayer.ActorNumber);
    }

    private int IdentifyPlayer ()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            if (PhotonNetwork.PlayerList[i].ActorNumber == pview.ControllerActorNr)
                return i;

        return -1;
    }
}
