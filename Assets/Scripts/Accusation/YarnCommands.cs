using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Yarn.Unity;

[RequireComponent(typeof(PhotonView))]
public class YarnCommands : MonoBehaviour
{
    
    private PhotonView _photonView;
    private InMemoryVariableStorage _variableStorage;

    private void Awake()
    {
        _variableStorage = GameObject.FindGameObjectWithTag("YarnMemory").GetComponent<InMemoryVariableStorage>();
        _photonView = GetComponent<PhotonView>();
    }

    [YarnCommand("SetBool")]
    public void SetBool(string yarnString, bool val)
    {
        _photonView.RPC(nameof(SyncVal), RpcTarget.Others, yarnString, val);
    }

    [PunRPC]
    public void SyncVal(string yarnString, bool val)
    {
        _variableStorage.SetValue(yarnString, val);
    }
}
