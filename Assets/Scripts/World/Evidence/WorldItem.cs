using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Journal;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(PhotonView))]
public class WorldItem : MonoBehaviour, IInteractable
{
    public Evidence evidence_SO;
    public GameObject itemPair;
    public bool isPair;

    private InMemoryVariableStorage memoryVariableStorage;
    private PhotonView _photonView;
    private bool _hasFlashed;

    private void Awake()
    {
        memoryVariableStorage = GameObject.FindGameObjectWithTag("YarnMemory").GetComponent<InMemoryVariableStorage>();
        _photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        EventManager.I.OnPlayersSpawned += Setup;
    }

    private void OnDisable()
    {
        EventManager.I.OnPlayersSpawned -= Setup;
    }

    private void Setup(GameObject humanGO, GameObject dogGO)
    {
        if(isPair) gameObject.SetActive(false);
    }

    public void Interact()
    {
        if (HumanCanvas.current != null && !_hasFlashed)
        {
            _hasFlashed = true;
            HumanCanvas.current.FlashJournal();
        }
        if (!string.IsNullOrEmpty(evidence_SO.promptMessageHuman) && PlayerManager.ThisPlayer == PlayerSpecies.Human) TextAppearerer.current.PromptPlayer(PlayerSpecies.Human, evidence_SO.promptMessageHuman);
        if (!string.IsNullOrEmpty(evidence_SO.promptMessageDog) && PlayerManager.ThisPlayer == PlayerSpecies.Dog) TextAppearerer.current.PromptPlayer(PlayerSpecies.Dog, evidence_SO.promptMessageDog);

        _photonView.RPC(nameof(Interact_RPC), RpcTarget.AllBuffered);
        _photonView.RPC(nameof(ShowPairItem), RpcTarget.AllBuffered);

        if(PlayerManager.ThisPlayer == PlayerSpecies.Human) _photonView.RPC(nameof(SetYarnString_RPC), RpcTarget.AllBuffered, evidence_SO.yarnString);
        if(PlayerManager.ThisPlayer == PlayerSpecies.Dog) _photonView.RPC(nameof(SetYarnString_RPC), RpcTarget.AllBuffered, evidence_SO.dogYarnString);
    }

    public bool CanInteract()
    {
        return !_hasFlashed;
    }

    [PunRPC]
    private void ShowPairItem()
    {
        if (itemPair != null)
        {
            gameObject.SetActive(false);
            itemPair.SetActive(true);
        }
    }
    
    [PunRPC]
    private void Interact_RPC()
    {
        evidence_SO.isFound = true;
    }

    [PunRPC]
    private void SetYarnString_RPC(string yarnString)
    {
        if (!string.IsNullOrEmpty(yarnString))
        {
            memoryVariableStorage.SetValue(yarnString, true);
        }
    }
}
