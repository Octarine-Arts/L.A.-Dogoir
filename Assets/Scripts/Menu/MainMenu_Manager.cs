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
            if (child.TryGetComponent(out Menu menu))
            {
                if (menu.shouldBeOpen) listOfMenus.Add(child.gameObject);
            }
            else
            {
                listOfMenus.Add(child.gameObject);
            }
            
            
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
        if (_currentMenuIndex + 1 >= listOfMenus.Count) return;
        
        listOfMenus[_currentMenuIndex].SetActive(false);
        _currentMenuIndex++;
        listOfMenus[_currentMenuIndex].SetActive(true);
    }

    public void OpenPreviousPage()
    {
        if (_currentMenuIndex - 1 < 0) return;
        
        listOfMenus[_currentMenuIndex].SetActive(false);
        _currentMenuIndex--;
        listOfMenus[_currentMenuIndex].SetActive(true); ;
    }

    public bool IsFirstPage() => _currentMenuIndex == 0;
    public bool IsLastPage() => _currentMenuIndex == listOfMenus.Count - 1;
}
