using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneContentSetter : MonoBehaviour
{
    public Cutscene_Manager cutscene;

    public SerialisableStringListKeyValuePair[] serailisedEventLists;
    private Dictionary<string, CutsceneList> eventLists = new Dictionary<string, CutsceneList>();

    private void Awake()
    {
        eventLists = new Dictionary<string, CutsceneList>(serailisedEventLists.Length);
        for (int i = 0; i < serailisedEventLists.Length; i++)
            eventLists.Add(serailisedEventLists[i].key, serailisedEventLists[i].list);
    }

    public void SetContent(string key) => cutscene.eventList = eventLists[key];

    [System.Serializable]
    public class SerialisableStringListKeyValuePair
    {
        public string key;
        public CutsceneList list;
    }
}
