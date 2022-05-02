using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Photon.Pun;

public class TriggerPrompt : MonoBehaviour
{
    public string humanMessage;
    public string dogMessage;

    private bool _isOnCooldown;
    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
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
        StartCoroutine(Cooldown());
        if(PlayerManager.ThisPlayer == PlayerSpecies.Human) TextAppearerer.current.PromptPlayer(PlayerSpecies.Human, message);
        else TextAppearerer.current.PromptPlayer(PlayerSpecies.Dog, message);
    }

    private IEnumerator Cooldown()
    {
        _isOnCooldown = true;
        yield return new WaitForSeconds(10f);
        _isOnCooldown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isOnCooldown) return;

        if(other.CompareTag("HumanAgent"))
        {
            PromptPlayer(humanMessage);
        }   
        else if(other.CompareTag("DogAgent"))
        {
            PromptPlayer(dogMessage);
        }
    }
}
