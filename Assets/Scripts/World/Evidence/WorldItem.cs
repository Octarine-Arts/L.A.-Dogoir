using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class WorldItem : MonoBehaviour, IInteractable
{
    public Evidence evidence_SO;

    private InMemoryVariableStorage memoryVariableStorage;

    private void Awake()
    {
        memoryVariableStorage = GameObject.FindGameObjectWithTag("YarnMemory").GetComponent<InMemoryVariableStorage>();
    }

    public void Interact()
    {
        //memoryVariableStorage.SetValue(evidence_SO.yarnString, true);   
        evidence_SO.isFound = true;
        Destroy(gameObject);
    }
}
