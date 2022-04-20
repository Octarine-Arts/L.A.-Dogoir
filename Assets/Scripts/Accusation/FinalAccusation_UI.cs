using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalAccusation_UI : MonoBehaviour
{
    public Transform dropdownParent;
    
    private List<EvidenceDropdown> _dropdownList;
    private FinalAccusation _finalAccusation;

    private List<Evidence> _currentEvidenceList;

    private void Awake()
    {
        _finalAccusation = GetComponent<FinalAccusation>();
        _dropdownList = new List<EvidenceDropdown>();
        foreach (Transform child in dropdownParent)
        {
            EvidenceDropdown dropdown = child.GetComponent<EvidenceDropdown>();
            _dropdownList.Add(dropdown);
            dropdown.Initialise(this);
        }
    }

    private void OnEnable()
    {
        UpdateDropdownOptions();
    }

    private void GetEvidenceOptions()
    {
        //if (PlayerManager.ThisPlayer == PlayerSpecies.Human) 
        _currentEvidenceList = _finalAccusation.GetHumanEvidenceList();
        //else if (PlayerManager.ThisPlayer == PlayerSpecies.Dog) _currentEvidenceList = _finalAccusation.GetDogEvidenceList();
    }

    private void UpdateDropdownOptions()
    {
        GetEvidenceOptions();
        foreach (var t in _dropdownList)
        {
            t.SetOptions(_currentEvidenceList);
        }
    }

    public void CheckEvidence()
    {
        List<Evidence> evidenceList = new List<Evidence>();
        foreach (EvidenceDropdown dropdown in _dropdownList)
        {
            evidenceList.Add(dropdown.GetCurrentEvidence());
        }
            
        Debug.Log(_finalAccusation.CheckEvidence(evidenceList));
    }
}
