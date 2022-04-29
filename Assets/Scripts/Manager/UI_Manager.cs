using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UI_Manager
{
    public static bool _isUIOpen;
    public static string _currentMenu;

    public static event Action ONUIOpen;
    public static void SetIsOpen(bool isOpen, string menuName)
    {
        _isUIOpen = isOpen;
        if (isOpen)
        {
            _currentMenu = menuName;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (!isOpen)
        {
            _currentMenu = "";
            Cursor.lockState = CursorLockMode.Locked;
            ONUIOpen?.Invoke();
        }
    }
}
