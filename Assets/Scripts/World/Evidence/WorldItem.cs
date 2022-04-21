using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(PhotonView))]
public class WorldItem : MonoBehaviour, IInteractable
{
    public Evidence evidence_SO;

    private InMemoryVariableStorage memoryVariableStorage;
    private PhotonView _photonView;
    
    private void Awake()
    {
        memoryVariableStorage = GameObject.FindGameObjectWithTag("YarnMemory").GetComponent<InMemoryVariableStorage>();
        _photonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void Interact()
    {
        evidence_SO.isFound = true;
        if (HumanCanvas.current != null) HumanCanvas.current.FlashJournal();

        if (string.IsNullOrEmpty(evidence_SO.yarnString)) return;
        _photonView.RPC(nameof(Interact), RpcTarget.Others);
        memoryVariableStorage.SetValue(evidence_SO.yarnString, true);
    }
}
