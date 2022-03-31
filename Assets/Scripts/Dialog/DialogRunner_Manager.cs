using System.Collections;
using System.Collections.Generic;
using Journal;
using UnityEngine;
using Yarn.Unity;

public static class DialogRunner_Manager
{
    public static void AddScriptToDialogueRunner(DialogueRunner currentRunner, YarnProject script)
    {
        currentRunner.yarnProject = script;
    }
}
