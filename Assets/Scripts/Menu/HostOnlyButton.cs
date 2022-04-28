using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HostOnlyButton : MonoBehaviourPunCallbacks
{
    private void OnEnable()
    {
        Debug.Log("ASd");
        gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }
}
