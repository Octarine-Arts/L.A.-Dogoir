using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade current;
    
    private Animator _animator;
    private bool _isBlack;
    
    private void Awake()
    {
        current = this;
        _animator = GetComponent<Animator>();
    }

    public bool IsBlack()
    {
        return _isBlack;
    }

    public void SetIsBlack()
    {
        _isBlack = true;
    }

    public void SetIsTransparent()
    {
        _isBlack = false;
    }
    
    public void FadeToBlack()
    {
        _animator.SetTrigger("Fade_Black");
    }

    public void FadeToTransparent()
    {
        _animator.SetTrigger("Fade_Transparent");
    }
}
