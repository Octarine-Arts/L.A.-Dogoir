using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private string _menuName;

    private void Awake()
    {
        _menuName = name;
    }
    
    public string GetMenuName() => _menuName;
    public void OpenMenu() => transform.GetChild(0).gameObject.SetActive(true);
    public void CloseMenu() => transform.GetChild(0).gameObject.SetActive(false);
}
