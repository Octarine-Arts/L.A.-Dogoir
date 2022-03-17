using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvidencePage : MonoBehaviour
{
    public Image evidenceImage;
    public TMP_Text evidenceName;
    public TMP_Text evidenceDescription;

    public GameObject buttonGO;
    public Transform buttonParent;
    
    private List<BarEvidence_SO> _listOfEvidence;
    private List<GameObject> _evidenceButtons;

    private void Awake()
    {
        _evidenceButtons = new List<GameObject>();
        _listOfEvidence = new List<BarEvidence_SO>();

        foreach (var evidence in Resources.LoadAll("Evidence_Bar"))
        {
            _listOfEvidence.Add((BarEvidence_SO) evidence);
        }
        
        CreateButtons();
    }

    private void CreateButtons()
    {
        for (int ii = 0; ii < _listOfEvidence.Count; ii++)
        {
            int index = new int();
            index = ii;
            
            GameObject instantiatedObject = Instantiate(buttonGO, buttonParent);
            instantiatedObject.GetComponentInChildren<TMP_Text>().text = _listOfEvidence[ii].displayName;
            instantiatedObject.GetComponent<Button>().onClick.AddListener(delegate { ShowEvidenceDetails(index); });
            instantiatedObject.SetActive(false);
            
            _evidenceButtons.Add(instantiatedObject);
        }
    }

    public void EvidenceFound(string evidenceName)
    {
        for (int ii = 0; ii < _listOfEvidence.Count; ii++)
        {
            if (_listOfEvidence[ii].displayName == evidenceName)
            {
                _evidenceButtons[ii].SetActive(true);
            }
        }
    }

    private void ShowEvidenceDetails(int index)
    {
        evidenceImage.sprite = _listOfEvidence[index].displayImage;
        evidenceName.text = _listOfEvidence[index].displayName;
        evidenceDescription.text = _listOfEvidence[index].description;
    }
}
