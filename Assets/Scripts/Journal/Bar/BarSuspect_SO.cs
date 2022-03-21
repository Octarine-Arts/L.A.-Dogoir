using System;
using System.Collections.Generic;
using UnityEngine;
using Journal.Bar;

namespace Journal
{
    [CreateAssetMenu(fileName = "Suspect", menuName = "ScriptableObjects/SuspectObject")]
    public class BarSuspect_SO : ScriptableObject
    {
        [Header("Uneditable By Player")]
        public Sprite image;
        public string fullName;
        public string description;
        public string occupation;
        public string personality;

        public Motives correctMotive;
        public BarLocation_SO correctLocation;
        public SuspectColor correctColor;

        [Header("Editable By Player")]
        public Motives motive = Motives.None;
        public Locations location;
        public SuspectColor color = SuspectColor.None;

        [Header("Testimonies")] 
        public List<Testimony_SO> testimonies;
        private bool[] _testimonyHeard;
        
        public bool IsCorrectMotive() => correctMotive == motive;
        public bool IsCorrectLocation() => correctLocation.locationEnum == location;
        public bool IsCorrectColor() => correctColor == color;

        private void OnValidate()
        {
            _testimonyHeard = new bool[testimonies.Count];
        }

        public List<Testimony_SO> GetHeardTestimonies()
        {
            List<Testimony_SO> listOfTestimonies = new List<Testimony_SO>();
                
            for (int ii = 0; ii < testimonies.Count; ii++)
            {
                // if (_testimonyHeard[ii])
                // {
                //     listOfTestimonies.Add(testimonies[ii]);
                // }
                
                listOfTestimonies.Add(testimonies[ii]);
            }

            return listOfTestimonies;
        }
    }
}