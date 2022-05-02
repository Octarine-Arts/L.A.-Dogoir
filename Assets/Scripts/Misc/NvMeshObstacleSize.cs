using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NvMeshObstacleSize : MonoBehaviour
{
    public Vector3 humanPosition;
    public Vector3 dogPosition;
    public Vector3 dogSize;
    private NavMeshObstacle _navMeshObstacle;

    private void Awake()
    {
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
    }

    private void OnEnable()
    {
        EventManager.I.OnPlayersSpawned += Setup;
    }

    private void OnDisable()
    {
        EventManager.I.OnPlayersSpawned -= Setup;
    }

    private void Setup(GameObject human, GameObject dog)
    {
        if (PlayerManager.ThisPlayer == PlayerSpecies.Dog)
        {
            _navMeshObstacle.center = dogPosition;
            _navMeshObstacle.size = dogSize;
        }
        else
        {
            _navMeshObstacle.center = humanPosition;
        }
    }

}
