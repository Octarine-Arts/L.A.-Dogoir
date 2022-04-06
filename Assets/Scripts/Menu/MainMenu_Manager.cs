using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Manager : MonoBehaviour
{
    public static MainMenu_Manager current;

    public Transform menuParent;
    
    private List<GameObject> listOfMenus;
    private int _currentMenuIndex;
    
    private void Awake()
    {
        current = this;

        listOfMenus = new List<GameObject>();
        foreach (Transform child in menuParent)
        {
            listOfMenus.Add(child.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OpenMenu("Contents Page");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllMenus();
        }
    }

    public void OpenMenu(string menuName)
    {
        for (int ii = 0; ii < listOfMenus.Count; ii++)
        {
            if (listOfMenus[ii].name == menuName)
            {
                listOfMenus[ii].SetActive(true);
                _currentMenuIndex = ii;
            }
            else
            {
                listOfMenus[ii].SetActive(false);
            }
        }
    }
    
    public void CloseAllMenus()
    {
        foreach (GameObject menu in listOfMenus)
        {
            menu.SetActive(false);
        }
    }

    public void OpenNextPage()
    {
        listOfMenus[_currentMenuIndex].SetActive(false);
        _currentMenuIndex++;
        listOfMenus[_currentMenuIndex].SetActive(true);
    }

    public void OpenPreviousPage()
    {
        listOfMenus[_currentMenuIndex].SetActive(false);
        _currentMenuIndex--;
        listOfMenus[_currentMenuIndex].SetActive(true);
    }
}
