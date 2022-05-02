using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class NPC_Controller : MonoBehaviour
{
    //Transform that NPC has to follow
    public Transform transformToFollow;
    public InteractionIndicator interactionIndicator;
    public TriggerDialogue triggerDialogue;
    public float followingTriggerRadius;
    //NavMesh Agent variable
    private PhotonView _photonView;
    private NavMeshAgent _agent;
    private Animator _animator;
    private bool _isFollowing;
    private float unfollowedTriggerRadius;

    private void Awake()
    {
        EventManager.I.OnPlayersSpawned += SetFollow;
    }

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _photonView = GetComponent<PhotonView>();
        unfollowedTriggerRadius = triggerDialogue.distanceToTrigger;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_isFollowing);
        _animator.SetBool("isWalking", _agent.velocity.magnitude > 0.1f);
        if (!_isFollowing) return;
        _agent.destination = transformToFollow.position;
    }

    private void SetFollow(GameObject humanPlayer, GameObject dogPlayer)
    {
        transformToFollow = dogPlayer.transform;
    }
    
    [YarnCommand("Follow")]
    public void TriggerFollowing()
    {
        _isFollowing = true;
        _photonView.RPC(nameof(TriggerFollowRPC), RpcTarget.Others);
        interactionIndicator.radius = followingTriggerRadius;
        triggerDialogue.distanceToTrigger = followingTriggerRadius;
    }

    [YarnCommand("Unfollow")]
    public void TriggerUnfollow()
    {
        _isFollowing = false;
        _photonView.RPC(nameof(TriggerUnfollowRPC), RpcTarget.Others);
        interactionIndicator.radius = unfollowedTriggerRadius;
        triggerDialogue.distanceToTrigger = unfollowedTriggerRadius;
    } 

    [PunRPC]
    private void TriggerFollowRPC()
    {
        _isFollowing = true;
    }

    [PunRPC]
    private void TriggerUnfollowRPC()
    {
        _isFollowing = false;
    }

    private void OnDisable()
    {
        EventManager.I.OnPlayersSpawned -= SetFollow;
    }
}