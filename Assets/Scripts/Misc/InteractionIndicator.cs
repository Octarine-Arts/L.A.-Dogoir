using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionIndicator : MonoBehaviour
{
    public float radius;
    public bool showForHuman, showForDog;

    private Transform targetPlayer;
    private Animator anim;
    private bool indicatorActive = false;

    private void Awake ()
    {
        if (EventManager.I != null)
            EventManager.I.OnPlayersSpawned += OnPlayersSpawned;

        anim = GetComponent<Animator> ();
    }

    private void Update ()
    {
        if (targetPlayer == null) return;

        bool inRange = F.FastDistance (targetPlayer.position, transform.position) < radius * radius;
        if (indicatorActive && !inRange)
        {
            anim.SetBool ("Active", false);
            indicatorActive = false;
        }
        else if (!indicatorActive && inRange)
        {
            anim.SetBool ("Active", true);
            indicatorActive = true;
        }
    }

    private void OnPlayersSpawned (GameObject human, GameObject dog) //=> 
    {
        if (PlayerManager.ThisPlayer == PlayerSpecies.Human && !showForHuman) Destroy (gameObject);
        else if (PlayerManager.ThisPlayer == PlayerSpecies.Dog && !showForDog) Destroy (gameObject);

        targetPlayer = PlayerManager.ThisPlayer == PlayerSpecies.Human ? human.transform : dog.transform;
        print ($"this player: {PlayerManager.ThisPlayer}, humanGO: {human.name}, dogGO: {dog.name}");
    }
    //targetPlayer = PlayerManager.ThisPlayer == PlayerSpecies.Human ? human.transform : dog.transform;

    private void OnDrawGizmos ()
    {
        Gizmos.DrawWireSphere (transform.position, radius);
    }
}
