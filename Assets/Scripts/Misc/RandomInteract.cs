using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInteract : MonoBehaviour, IInteractable
{
    private bool _hasInteracted;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        _animator.SetTrigger("Play");
        _hasInteracted = true;
    }

    public bool CanInteract()
    {
        return !_hasInteracted;
    }
}
