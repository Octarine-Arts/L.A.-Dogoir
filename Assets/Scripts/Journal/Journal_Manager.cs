using System;
using System.Collections;
using System.Collections.Generic;
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
}
