using UnityEngine;
using Journal.Bar;

namespace Journal.Bar
{
    [CreateAssetMenu(fileName = "BarLocation", menuName = "ScriptableObjects/BarLocationObject")]
    public class BarLocation_SO : ScriptableObject
    {
        public Locations locationEnum;

        public string locationName;
    }
}