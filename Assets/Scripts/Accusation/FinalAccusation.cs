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

    public List<Evidence> _locationEvidence;
    public List<Evidence> _weaponEvidence;
    public List<Evidence> _suspectEvidence;
    public List<Evidence> _motiveEvidence;

    private List<Evidence> _humanEvidenceList = new();
    private List<Evidence> _dogEvidenceList = new();
    
    private void Awake()
    {
        _currentPhase = AccusationPhase.Location;

        foreach (var evidence in Resources.LoadAll("Bar/Evidences"))
        {
            _humanEvidenceList.Add((Evidence) evidence);
        }
        
        foreach (var evidence in Resources.LoadAll("Bar/DogEvidences"))
        {
            _dogEvidenceList.Add((Evidence) evidence);
        }
    }

    public bool CheckEvidence(List<Evidence> currentEvidenceList)
    {
        int numEvidence = 0;
        List<Evidence> currentListToCheck = new List<Evidence>();
        switch (_currentPhase)
        {
            case AccusationPhase.Location:
                numEvidence = _locationEvidence.Count;
                currentListToCheck = _locationEvidence;
                break;
            case AccusationPhase.Weapon:
                numEvidence = _weaponEvidence.Count;
                currentListToCheck = _weaponEvidence;
                break;
            case AccusationPhase.Suspect:
                numEvidence = _suspectEvidence.Count;
                currentListToCheck = _suspectEvidence;
                break;
            case AccusationPhase.Motive:
                numEvidence = _motiveEvidence.Count;
                currentListToCheck = _motiveEvidence;
                break;
        }

        if (numEvidence == currentEvidenceList.Count)
        {
            int numCorrect = 0;
            foreach (Evidence evidence in currentEvidenceList)
            {
                if (evidence == null) continue;
                if (currentListToCheck.Contains(evidence)) numCorrect++;
            }

            if (numCorrect == currentListToCheck.Count) return true;
        }

        return false;
    }
    
    public List<Evidence> GetHumanEvidenceList()
    {
        List<Evidence> foundEvidence = new List<Evidence>();
        foreach (Evidence evidence in _humanEvidenceList)
        {
            if(evidence.isFound) foundEvidence.Add(evidence);
        }

        return foundEvidence;
    }
    
    public List<Evidence> GetDogEvidenceList()
    {
        List<Evidence> foundEvidence = new List<Evidence>();
        foreach (Evidence evidence in _dogEvidenceList)
        {
            if(evidence.isFound) foundEvidence.Add(evidence);
        }

        return foundEvidence;
    }
}
