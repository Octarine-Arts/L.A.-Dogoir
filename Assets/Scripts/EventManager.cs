using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager I;

    private GameObject humanPlayer, dogPlayer;
    public event Action<GameObject, GameObject> OnPlayersSpawned;
    public void PlayerSpawned (PlayerSpecies player, GameObject gameObject)
    {
        if (player == PlayerSpecies.Human) humanPlayer = gameObject;
        else dogPlayer = gameObject;

        if (dogPlayer != null && humanPlayer != null) OnPlayersSpawned?.Invoke (humanPlayer, dogPlayer);
    }

    private void Awake ()
    {
        if (I != null)
        {
            Destroy (gameObject);
            return;
        }
        else
        {
            I = this;
            DontDestroyOnLoad (gameObject);
        }
    }
}
