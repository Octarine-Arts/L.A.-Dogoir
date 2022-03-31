using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using Journal.Pages;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Journal.Pages
{
    public class TestimonyPage : MonoBehaviour
    {
    
        public GameObject suspectButtonGO;
        public Transform row1Parent;
        public Transform row2Parent;
    
        public GameObject testimonyButtonGO;
        public Transform testimonyListParent;
    
        private List<Suspect> _suspectList;
        
        private void Awake()
        {
            _suspectList = new List<Suspect>();
            
            foreach (var suspect in Resources.LoadAll(Journal_Manager.current.suspectsPath))
            {
                _suspectList.Add((Suspect) suspect);
            }
            CreateButtons();
        }

        private void CreateButtons()
        {
            for (int ii = 0; ii < _suspectList.Count; ii++)
            {
                GameObject instantiatedObject = null;
                var index = ii;
    
                switch (ii % 2)
                {
                    case 0:
                        instantiatedObject = Instantiate(suspectButtonGO, row1Parent);
                        break;
                    case 1:
                        instantiatedObject = Instantiate(suspectButtonGO, row2Parent);
                        break;
                }
    
                if (instantiatedObject == null) return;
                instantiatedObject.GetComponent<Image>().sprite = _suspectList[ii].displayImage;
                instantiatedObject.GetComponent<Button>().onClick.AddListener(delegate { ShowTestimonies(index); });
                instantiatedObject.GetComponentInChildren<TMP_Text>().text = _suspectList[ii].fullName;
    
            }
        }
    
        private void ShowTestimonies(int index)
        {
            ClearTestimonyList();
            List<Suspect.Testimony> listOfTestimony = _suspectList[index].GetAllFoundTestimonies();
    
            foreach (var testimony in listOfTestimony)
            {
                GameObject instantiatedObject = Instantiate(testimonyButtonGO, testimonyListParent);
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
}
