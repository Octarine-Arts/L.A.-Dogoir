using UnityEngine;

namespace Journal
{
    [CreateAssetMenu(fileName = "Motive", menuName = "Journal/MotiveObject")]
    public class Motive : ScriptableObject
    {
        public Motives motive;
        public string displayName;
    }
}