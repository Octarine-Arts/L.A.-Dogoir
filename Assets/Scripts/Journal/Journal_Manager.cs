using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using Unity.VisualScripting;
using UnityEngine;

public class Journal_Manager : MonoBehaviour
{
    public static Journal_Manager current;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("ASd");
            _menuManager.OpenMenu("Contents Page");
            Cursor.lockState = CursorLockMode.None;
            Player_StaticActions.DisableHumanMovement();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Player_StaticActions.EnableHumanMovement();
            _menuManager.CloseAllMenus();
        }
    }

    private void RemoveObject()
    {
        //if (PlayerManager.ThisPlayer == PlayerSpecies.Dog) Destroy(gameObject);
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
