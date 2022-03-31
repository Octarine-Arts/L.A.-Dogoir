using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using Unity.VisualScripting;
using UnityEngine;

public class WorldItem : MonoBehaviour, IInteractable
{
    public Evidence evidence_SO;

    public void Interact()
    {
        evidence_SO.isFound = true;
        Destroy(gameObject);
    }
}
