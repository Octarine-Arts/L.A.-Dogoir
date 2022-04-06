using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager I;

    public event Action<PlayerSpecies, GameObject> OnPlayerSpawned;
    public void PlayerSpawned (PlayerSpecies player, GameObject gameObject) => OnPlayerSpawned?.Invoke (player, gameObject);

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
