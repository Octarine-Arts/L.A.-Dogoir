using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using Journal.Bar;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TestimonyPage : MonoBehaviour
{
    public List<BarSuspect_SO> suspectList;

    public GameObject suspectButtonGO;
    public Transform row1Parent;
    public Transform row2Parent;

    public GameObject testimonyButtonGO;
    public Transform testimonyListParent;

    private void Awake()
    {
        CreateButtons();
    }

    private void CreateButtons()
    {
        for (int ii = 0; ii < suspectList.Count; ii++)
        {
            GameObject instantiatedObject = new GameObject();
            int index = new int();
            index = ii;
            
            switch (ii % 2)
            {
                case 0:
                    instantiatedObject = Instantiate(suspectButtonGO, row1Parent);
                    break;
                case 1:
                    instantiatedObject = Instantiate(suspectButtonGO, row2Parent);
                    break;
            }

            instantiatedObject.GetComponent<Image>().sprite = suspectList[ii].image;
            instantiatedObject.GetComponent<Button>().onClick.AddListener(delegate { ShowTestimonies(index); });
            instantiatedObject.GetComponentInChildren<TMP_Text>().text = suspectList[ii].fullName; ;
        }
    }

    private void ShowTestimonies(int index)
    {
        ClearTestimonyList();
        List<Testimony_SO> listOfTestimony = suspectList[index].GetHeardTestimonies();
        GameObject instantiatedObject;
        
        foreach (var testimony in listOfTestimony)
        {
            instantiatedObject = Instantiate(testimonyButtonGO, testimonyListParent);
            instantiatedObject.GetComponent<TestimonyButton>().SetupButton(testimony.testimonyTitle, testimony.testimonyDescription);
        }
    }

    private void ClearTestimonyList()
    {
        foreach (Transform child in testimonyListParent)
        {
            Destroy(child.gameObject);
        }
    }
}
