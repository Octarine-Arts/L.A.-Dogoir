using System.Collections;
using System.Collections.Generic;
using Journal;
using UnityEngine;

namespace Journal
{
    [CreateAssetMenu(fileName = "Evidence", menuName = "Journal/EvidenceObject")]
    public class Evidence : ScriptableObject
    {
        [Header("Default Values")]
        public Sprite displayImage;
        public string displayName;
        public Locations locationFound;
        public string description;

        public bool isFound;

        [Header("Player Input")] 
        public Suspects currentSuspect;
        public SuspectColors currentColor;
    }
}
