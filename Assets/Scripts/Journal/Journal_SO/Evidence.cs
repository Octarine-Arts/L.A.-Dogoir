using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using Unity.VisualScripting;
using UnityEngine;

namespace Journal
{
    [CreateAssetMenu(fileName = "Evidence", menuName = "Journal/EvidenceObject")]
    public class Evidence : ScriptableObject
    {
        [Header("Default Values")]
        public Sprite displayImage;
        public string displayName;
        public Location locationFound;
        public string description;


        [Header("Player Input")] 
        public bool isFound;

        public void ResetValues()
        {
            isFound = false;
        }
    }
}
