using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Yarn.Unity;

public class YarnToAnim : MonoBehaviour
{
    public string defaultTrigger;
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        if(!string.IsNullOrEmpty(defaultTrigger)) _animator.SetTrigger(defaultTrigger);
    }

    [YarnCommand("Animate")]
    public void Animate(string command)
    {
        _animator.SetTrigger(command);
    }
}
