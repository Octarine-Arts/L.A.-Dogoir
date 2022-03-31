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
    public TMP_Text evidenceLocation;
    
    public GameObject buttonGO;
    public Transform buttonParent;
    
    private List<Evidence> _listOfEvidence;

    private void Awake()
    {
        _listOfEvidence = new List<Evidence>();

        foreach (var evidence in Resources.LoadAll(Journal_Manager.current.evidencePath))
        {
            _listOfEvidence.Add((Evidence) evidence);
        }
    }

    private void OnEnable()
    {
        CreateButtons();
    }

    private void CreateButtons()
    {
        ClearButtons();
        for (int ii = 0; ii < _listOfEvidence.Count; ii++)
        {
            int index = ii;
            
            GameObject instantiatedObject = Instantiate(buttonGO, buttonParent);
            instantiatedObject.GetComponentInChildren<TMP_Text>().text = _listOfEvidence[ii].displayName;
            instantiatedObject.GetComponent<Button>().onClick.AddListener(delegate { ShowEvidenceDetails(index); });
            instantiatedObject.SetActive(_listOfEvidence[ii].isFound);
        }
    }

    private void ClearButtons()
    {
        foreach (Transform trans in buttonParent)
        {
            Destroy(trans.gameObject);
        }
    }
    
    private void ShowEvidenceDetails(int index)
    {
        evidenceImage.sprite = _listOfEvidence[index].displayImage;
        evidenceName.text = _listOfEvidence[index].displayName;
        evidenceLocation.text = "Location: " + _listOfEvidence[index].locationFound.displayName;
        evidenceDescription.text = _listOfEvidence[index].description;
    }
}
