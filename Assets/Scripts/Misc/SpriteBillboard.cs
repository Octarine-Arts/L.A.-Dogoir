using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField] private bool backwards;
    private Transform cam;

    private void OnPlayersSpawned (GameObject human, GameObject dog)
    {
        cam = (PlayerManager.ThisPlayer == PlayerSpecies.Human ? human : dog).transform.parent.GetComponentInChildren<Camera> ().transform;
    }

    private void Update ()
    {
        //transform.LookAt (backwards ? transform.position - cam.position : cam.position);
        if (cam == null) return;
        transform.forward = backwards ? cam.forward : -cam.forward;
    }

    private void OnEnable()
    {
        if (cam != null) return;

        if (PlayerManager.current.PlayersSpawned)
            cam = (PlayerManager.ThisPlayer == PlayerSpecies.Human ? PlayerManager.current.HumanPlayer : PlayerManager.current.DogPlayer).transform.parent.GetComponentInChildren<Camera>().transform;
        else if (EventManager.I != null)
            EventManager.I.OnPlayersSpawned += OnPlayersSpawned;
    }

    private void OnDisable()
    {
        EventManager.I.OnPlayersSpawned -= OnPlayersSpawned;
    }
}
