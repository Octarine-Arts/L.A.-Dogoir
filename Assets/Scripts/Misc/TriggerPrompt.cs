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
                StartCoroutine(PromptPlayer_CO(message1, 0f));
                StartCoroutine(PromptPlayer_CO(message2, 5f));
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

    private void PromptPlayer(string message)
    {
        if(PlayerManager.ThisPlayer == PlayerSpecies.Human) TextAppearerer.current.PromptPlayer(PlayerSpecies.Human, message);
    }

    private IEnumerator PromptPlayer_CO(string message, float delay)
    {
        yield return new WaitForSeconds(delay);
        PromptPlayer(message);
    }
    
    private IEnumerator Cooldown()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(10f);
        _isOnCooldown = false;
    }
}
