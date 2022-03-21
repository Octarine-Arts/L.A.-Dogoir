using System.Collections;
using System.Collections.Generic;
using Journal.Bar;
using UnityEngine;

namespace Journal
{
    [CreateAssetMenu(fileName = "BarEvidence", menuName = "ScriptableObjects/BarEvidenceObject")]
    public class BarEvidence_SO : ScriptableObject
    {
        public Sprite displayImage;
        public string displayName;
        public BarLocation_SO locationFound;
        public string description;

        public Suspects currentSuspect;
        public string currentColor;
    }
}

