using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnToAnim : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    [YarnCommand("Animate")]
    public void Animate(string command)
    {
        _animator.SetTrigger(command);
    }
}
