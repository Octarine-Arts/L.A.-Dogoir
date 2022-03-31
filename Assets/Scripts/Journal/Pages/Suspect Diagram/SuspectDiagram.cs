using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuspectDiagram : MonoBehaviour
{
    public static SuspectDiagram current;

    private int _correctCount;
    
    private void Awake()
    {
        current = this;
    }

    public void CorrectAnswer()
    {
        _correctCount++;
        if (_correctCount == 4)
        {
            CorrectGroup();
        }
    }

    public void WrongAnswer()
    {
        _correctCount--;
    }
    
    public event Action ONCorrectGroup;

    private void CorrectGroup()
    {
        ONCorrectGroup?.Invoke();
        _correctCount = 0;
    }
}
