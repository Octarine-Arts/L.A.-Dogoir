using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using Unity.VisualScripting;
using UnityEngine;

public class Journal_Manager : MonoBehaviour
{
    public static Journal_Manager current;

    public AudioClip nextPageSound;
    public string suspectsPath;
    public string evidencePath;
    public string locationPath;

    private MainMenu_Manager _menuManager;

    private void Awake()
    {
        current = this;
        _menuManager = GetComponent<MainMenu_Manager>();

        Invoke(nameof(RemoveObject), 2f);
    }

	private void Update()
    {
        if (!UI_Manager.enableUI) return;
        
        if (UI_Manager._isUIOpen && UI_Manager._currentMenu == "Journal")
        {
            CheckCloseMenu();
            CheckLRInput();
        }
        else if (!UI_Manager._isUIOpen)
        {
            CheckOpenMenu();
        }
    }

    private void CheckLRInput()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (_menuManager.IsLastPage()) return;
            _menuManager.OpenNextPage();
            AudioManager.current.PlaySFX(nextPageSound);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (_menuManager.IsFirstPage()) return;
            _menuManager.OpenPreviousPage();
            AudioManager.current.PlaySFX(nextPageSound);
        }
    }
    
    private void CheckOpenMenu()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            UI_Manager.SetIsOpen(true, "Journal");
            _menuManager.OpenMenu("Contents Page");
            Player_StaticActions.DisableHumanMovement();
        }
    }

    private void CheckCloseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.J))
        {
            UI_Manager.SetIsOpen(false, "Journal");
            Player_StaticActions.EnableHumanMovement();
            _menuManager.CloseAllMenus();
        }
    }
    
    private void RemoveObject()
    {
        if (PlayerManager.ThisPlayer == PlayerSpecies.Dog) Destroy(gameObject);
    }

    private void OnApplicationQuit()
    {
        foreach (var evidence in Resources.LoadAll<Evidence>(evidencePath))
        {
            evidence.ResetValues();
        }
        foreach (var suspect in Resources.LoadAll<Suspect>(suspectsPath))
        {
            suspect.ResetValues();
        }
    }
}
