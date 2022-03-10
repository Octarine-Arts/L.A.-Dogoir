using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Manager : MonoBehaviour
{
    public static MainMenu_Manager current;
    
    public List<GameObject> listOfMenus;

    private void Awake()
    {
        current = this;
    }

    public void OpenMenu(string menuName)
    {
        foreach (GameObject menu in listOfMenus)
        {
            menu.SetActive(menu.name == menuName);
        }
    }
}
