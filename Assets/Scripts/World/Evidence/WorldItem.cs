using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class WorldItem : MonoBehaviour, IInteractable
{
    public Evidence evidence_SO;

    private InMemoryVariableStorage memoryVariableStorage;
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        memoryVariableStorage = GameObject.FindGameObjectWithTag("YarnMemory").GetComponent<InMemoryVariableStorage>();
    }

    public void Interact()
    {
        //memoryVariableStorage.SetValue(evidence_SO.yarnString, true);   
        evidence_SO.isFound = true;
        photonView.RPC("ItemInteracted", RpcTarget.Others);
    }
}
