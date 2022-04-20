using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using UnityEngine;

public enum AccusationPhase
{
    Location,
    Weapon,
    Suspect,
    Motive
}

public class FinalAccusation : MonoBehaviour
{
    private AccusationPhase _currentPhase;

    private List<Evidence> _locationEvidence;
    private List<Evidence> _weaponEvidence;
    private List<Evidence> _suspectEvidence;
    private List<Evidence> _motiveEvidence;

    private List<Evidence> _currentEvidenceList;
    
    private void Awake()
    {
        _currentPhase = AccusationPhase.Location;
    }

    private bool CheckEvidence()
    {
        int numEvidence = 0;
        List<Evidence> currentListToCheck;
        switch (_currentPhase)
        {
            case AccusationPhase.Location:
                numEvidence = _locationEvidence.Count;
                break;
            case AccusationPhase.Weapon:
                numEvidence = _weaponEvidence.Count;
                break;
            case AccusationPhase.Suspect:
                numEvidence = _suspectEvidence.Count;
                break;
            case AccusationPhase.Motive:
                numEvidence = _motiveEvidence.Count;
                break;
        }

        if (numEvidence == _currentEvidenceList.Count)
        {
            int numCorrect;
            foreach (Evidence evidence in _currentEvidenceList)
            {
                
            }
        }

        return false;
    }
}
