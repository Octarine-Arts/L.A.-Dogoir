using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Photon.Pun;

public class TriggerPrompt : MonoBehaviour
{
    public string message1;
    public string message2;
    public string message3;
    
    private bool _isOnCooldown;
    private PhotonView _photonView;
    private GameObject _humanGO;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (_isOnCooldown) return;
        if (PlayerManager.ThisPlayer == PlayerSpecies.Human)
        {
            if(_humanGO == null) _humanGO = PlayerManager.current.HumanPlayer;
            if (Vector3.Distance(_humanGO.transform.position, transform.position) < 3f)
            {
                StartCoroutine(Cooldown());
                StartCoroutine(PromptPlayer_CO(PlayerSpecies.Human, message1, 0f));
                StartCoroutine(PromptPlayer_CO(PlayerSpecies.Human, message2, 5f));
                StartCoroutine(PromptPlayer_CO(PlayerSpecies.Dog, message3, 10f));
            }
        }
    }

    [YarnCommand("RemoveDoorCollider")]
    private void RemoveDoorCollider()
    {
        _photonView.RPC(nameof(RemoveDoorCollider_RPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RemoveDoorCollider_RPC()
    {
        Destroy(gameObject);
    }

    private void PromptPlayer(PlayerSpecies species, string message)
    {
        if(PlayerManager.ThisPlayer == species) TextAppearerer.current.PromptPlayer(species, message);
    }

    private IEnumerator PromptPlayer_CO(PlayerSpecies species, string message, float delay)
    {
        yield return new WaitForSeconds(delay);
        PromptPlayer(species, message);
    }
    
    private IEnumerator Cooldown()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(10f);
        _isOnCooldown = false;
    }
}
