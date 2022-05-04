using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInteract : MonoBehaviour, IInteractable
{
    private bool _hasInteracted;
    private RPCAnimTrigger _animator;

    private void Awake()
    {
        _animator = GetComponent<RPCAnimTrigger>();
    }

    public void Interact()
    {
        _animator.Trigger("Play");
        _hasInteracted = true;
    }

    public bool CanInteract()
    {
        return !_hasInteracted;
    }
}
