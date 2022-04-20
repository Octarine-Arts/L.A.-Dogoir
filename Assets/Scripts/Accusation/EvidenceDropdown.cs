using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceDropdown : MonoBehaviour
{
    private TMP_Dropdown _dropdown;
    private Evidence _currentEvidence;
    private List<Evidence> _listOfEvidence;
    private FinalAccusation_UI _finalAccusationUI;
    
    private void Awake()
    {
        _listOfEvidence = new List<Evidence>();
        _dropdown = GetComponent<TMP_Dropdown>();
        _dropdown.onValueChanged.AddListener(ValueChanged);
    }

    public void Initialise(FinalAccusation_UI ui)
    {
        _finalAccusationUI = ui;
    }
    
    public void SetOptions(List<Evidence> evidenceList)
    {
        string currentValue = _dropdown.options[_dropdown.value].text;
        _currentEvidence = evidenceList[0];
        _dropdown.options.Clear();
        _listOfEvidence.Clear();

        int value = 0;
        foreach (Evidence evidence in evidenceList)
        {
            _listOfEvidence.Add(evidence);
            _dropdown.options.Add(new TMP_Dropdown.OptionData {text = evidence.displayName});
            if (currentValue == evidence.displayName)
            {
                _currentEvidence = evidence;
                _dropdown.value = value;
            }
            value++;
        }
    }

    private void ValueChanged(int value)
    {
        string newAnswer = _dropdown.options[value].text;
        foreach (Evidence evidence in _listOfEvidence)
        {
            if (evidence.displayName == newAnswer)
            {
                _currentEvidence = evidence;
                break;
            }
        }
    }

    public Evidence GetCurrentEvidence() => _currentEvidence;
}
