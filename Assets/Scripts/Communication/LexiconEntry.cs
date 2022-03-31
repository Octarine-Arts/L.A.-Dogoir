using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Category", menuName = "ScriptableObjects/LexiconEntry")]
public class LexiconEntry : ScriptableObject, IEnumerable<string>
{
    [SerializeField] private string[] entries;

    public int Length => entries.Length;
    public string this[int i] => entries[i];

    public IEnumerator<string> GetEnumerator ()
    {
        foreach (string s in entries)
            yield return s;
    }

    IEnumerator IEnumerable.GetEnumerator () => GetEnumerator();
}
