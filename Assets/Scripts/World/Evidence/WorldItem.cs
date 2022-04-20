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
        if (string.IsNullOrEmpty(evidence_SO.yarnString)) return;
        
        memoryVariableStorage.TryGetValue(evidence_SO.yarnString, out bool isVariableTrue);
        if (isVariableTrue) return;
        
        memoryVariableStorage.SetValue(evidence_SO.yarnString, true);
        evidence_SO.isFound = true;
        _photonView.RPC(nameof(Interact), RpcTarget.Others);
    }
}
