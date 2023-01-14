using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class HumanCanvas : MonoBehaviour
{
    public static HumanCanvas current;

    public CanvasGroup journalImage;
    private Animator _animator;
    
    private void Awake()
    {
        current = this;
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void OnPlayersSpawned(GameObject human, GameObject dog)
    {
        if (PlayerManager.ThisPlayer == PlayerSpecies.Dog) Destroy(gameObject);
    }
    
    public void FlashJournal()
    {
        AudioManager.current.PlayWriteSound();
        StartCoroutine(Effects.FadeAlpha(journalImage, 0.5f, 1f, 0.5f));
        _animator.SetTrigger("ShowText");
    }
    
    private void Update()
    {
        print(UI_Manager._isUIOpen);
        if (UI_Manager._isUIOpen) GetComponent<Canvas>().enabled = false;
        else if (!UI_Manager._isUIOpen) GetComponent<Canvas>().enabled = true;
    }

    private void OnEnable()
    {
        EventManager.I.OnPlayersSpawned += OnPlayersSpawned;
    }

    private void OnDisable()
    {
        EventManager.I.OnPlayersSpawned -= OnPlayersSpawned;
    }
}
