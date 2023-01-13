using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorToManager : MonoBehaviour
{
    public Cutscene_Manager cutsceneManager;
    
    public void CutsceneFinished()
    {
        cutsceneManager.CutsceneEventFinished();
    }

    public void PlayNextEvent()
    {
        cutsceneManager.PlayNextEvent();
    }

    public void PlayAudio(AudioClip audioClip)
    {
        AudioManager.current.PlaySFX(audioClip);
    }
}
