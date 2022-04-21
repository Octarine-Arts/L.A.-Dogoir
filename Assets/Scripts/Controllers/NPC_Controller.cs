using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class NPCController : MonoBehaviour
{
    //Transform that NPC has to follow
    public Transform transformToFollow;
    //NavMesh Agent variable
    private NavMeshAgent _agent;
    private Animator _animator;
    private bool _isFollowing;

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