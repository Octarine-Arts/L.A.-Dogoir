using UnityEngine;

namespace Journal
{
    [CreateAssetMenu(fileName = "Location", menuName = "Journal/LocationObject")]
    public class Location : ScriptableObject
    {
        public Locations location;
        public string displayName;
    }
}