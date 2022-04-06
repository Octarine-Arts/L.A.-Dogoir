using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Journal
{
    [CreateAssetMenu(fileName = "Suspect", menuName = "Journal/SuspectObject")]

    public class Suspect : ScriptableObject
    {
        [Serializable]
        public class Testimony
        {
            public string testimonyTitle;
            [TextArea(2,5)]
            public string testimonyDescription;
            public bool testimonyHeard;
        }
        
        [Header("Default Values")]
        public Sprite displayImage;
        public Suspects suspectEnum;
        public string fullName;
        public string description;
        public string occupation;
        public string personality;

        public Motive correctMotive;
        public Location correctLocation;
        public SuspectColors correctColor;

        public List<Testimony> listOfTestimonies;

        [Header("Player Input")] 
        public Motive inputMotive;
        public Location inputLocation;
        public SuspectColors inputColor = SuspectColors.None;

        public bool hasTalked;

        public List<Testimony> GetAllFoundTestimonies()
        {
            List<Testimony> returnList = new List<Testimony>();
            foreach (Testimony testimony in listOfTestimonies)
            {
                if (testimony.testimonyHeard)
                {
                    returnList.Add(testimony);
                }
            }

            return returnList;
        }

        public void ResetValues()
        {
            hasTalked = false;
            inputMotive = null;
            inputLocation = null;
            inputColor = SuspectColors.None;
        }
    }
}

