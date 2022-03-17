using UnityEngine;

namespace Journal.Bar
{
    [CreateAssetMenu(fileName = "Testimony", menuName = "ScriptableObjects/TestimonyObject")]
    public class Testimony_SO : ScriptableObject
    {
        public string testimonyTitle;
        public string testimonyDescription;
    }
}