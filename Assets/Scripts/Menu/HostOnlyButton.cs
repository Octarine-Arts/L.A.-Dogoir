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
        gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }
}
