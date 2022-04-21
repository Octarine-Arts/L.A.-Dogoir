using System;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class NPC_Controller : MonoBehaviour
{
    //Transform that NPC has to follow
    public Transform transformToFollow;
    //NavMesh Agent variable
    private NavMeshAgent _agent;
    private Animator _animator;
    private bool _isFollowing;

    private void Awake()
    {
        EventManager.I.OnPlayersSpawned += SetFollow;
    }

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    [YarnCommand("Unfollow")]
    public void TriggerUnfollow()
    {
        _isFollowing = false;
    }
}