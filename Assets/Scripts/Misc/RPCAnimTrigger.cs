using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RPCAnimTrigger : MonoBehaviour
{
    [SerializeField] private string trigger;
    private Animator anim;
    private PhotonView pview;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        pview = GetComponent<PhotonView>();
    }

    public void Trigger() => pview.RPC("Trigger_RPC", RpcTarget.AllBuffered, trigger);
    public void Trigger(string trigger) => pview.RPC("Trigger_RPC", RpcTarget.AllBuffered, trigger);
    [PunRPC]
    private void Trigger_RPC(string trigger) => anim.SetTrigger(trigger);
}
