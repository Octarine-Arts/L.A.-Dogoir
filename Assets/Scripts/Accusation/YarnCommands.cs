using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Yarn.Unity;

[RequireComponent(typeof(PhotonView))]
public class YarnCommands : MonoBehaviour
{
    public static YarnCommands current;
    public AudioClip gunshotSound;
    
    private PhotonView _photonView;
    private InMemoryVariableStorage _variableStorage;

    private void Awake()
    {
        current = this;
        _variableStorage = GameObject.FindGameObjectWithTag("YarnMemory").GetComponent<InMemoryVariableStorage>();
        _photonView = GetComponent<PhotonView>();
    }

    [YarnCommand("SetBool")]
    public void SetBool(string yarnString, bool val)
    {
        _variableStorage.SetValue(yarnString, val);
        _photonView.RPC(nameof(SyncVal), RpcTarget.Others, yarnString, val);
    }

    [YarnCommand("TriggerGunshot")]
    public void TriggerGunshot()
    {
        AudioManager.current.PlaySFX(gunshotSound);
        if(PlayerManager.ThisPlayer == PlayerSpecies.Dog) GameObject.FindGameObjectWithTag("GunshotTrail").GetComponent<SmellTrail>().Activate();
        _photonView.RPC(nameof(Gunshot), RpcTarget.Others);
    }

    [PunRPC]
    public void SyncVal(string yarnString, bool val)
    {
        _variableStorage.SetValue(yarnString, val);
        Debug.Log(yarnString);
        Debug.Log(val);
    }

    [PunRPC]
    public void Gunshot()
    {
        AudioManager.current.PlaySFX(gunshotSound);
        if(PlayerManager.ThisPlayer == PlayerSpecies.Dog) GameObject.FindGameObjectWithTag("GunshotTrail").GetComponent<SmellTrail>().Activate();
    }
}
