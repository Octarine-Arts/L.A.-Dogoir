using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Yarn.Unity;

public class CustomLineView : LineView
{
    public bool isDetectiveBox;
    
    
    
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        if (isDetectiveBox && dialogueLine.CharacterName == "Detective")
        {
            base.RunLine(dialogueLine, onDialogueLineFinished);
        }
        else if(!isDetectiveBox && dialogueLine.CharacterName != "Detective")
        {
            base.RunLine(dialogueLine, onDialogueLineFinished);
        }
    }
}
