using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NvMeshObstacleSize : MonoBehaviour
{
    public Vector3 humanPosition;
    public Vector3 dogPosition;
    public Vector3 dogSize;
    private NavMeshObstacle navMeshObstacle;

    private void OnEnable()
    {
        EventManager.I.OnPlayersSpawned += Setup;
    }

    private void Setup(GameObject human, GameObject dog)
    {
        if (PlayerManager.ThisPlayer == PlayerSpecies.Dog)
        {
            navMeshObstacle.center = dogPosition;
            navMeshObstacle.size = dogSize;
        }
        else
        {
            navMeshObstacle.center = humanPosition;
        }
    }

}
