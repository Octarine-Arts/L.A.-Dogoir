using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using Unity.VisualScripting;
using UnityEngine;

public class Journal_Manager : MonoBehaviour
{
    public static Journal_Manager current;

    public string suspectsPath;
    public string evidencePath;
    public string locationPath;

    private void Awake()
    {
        current = this;
    }

    private void OnApplicationQuit()
    {
        foreach (var evidence in Resources.LoadAll<Evidence>(evidencePath))
        {
            evidence.ResetValues();
        }
        foreach (var suspect in Resources.LoadAll<Suspect>(suspectsPath))
        {
            suspect.ResetValues();
        }
    }
}
