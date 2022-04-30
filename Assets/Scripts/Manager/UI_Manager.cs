using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class UI_Manager
{
    public static bool enableUI;
    public static bool _isUIOpen;
    public static string _currentMenu;

    public static event Action ONUIOpen;

    public static void SetIsOpen(bool isOpen, string menuName)
    {
        _isUIOpen = isOpen;
        if (isOpen)
        {
            _currentMenu = menuName;
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(0)) Cursor.lockState = CursorLockMode.None;
        }
        else if (!isOpen)
        {
            _currentMenu = "";
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(0)) Cursor.lockState = CursorLockMode.Locked;
            ONUIOpen?.Invoke();
        }
    }
}
