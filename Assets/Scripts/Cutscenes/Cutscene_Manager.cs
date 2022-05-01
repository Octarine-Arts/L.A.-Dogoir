using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public enum CutsceneState
{
    NotStarted,
    Started,
    Playing,
    Waiting,
    Finished
}

public class Cutscene_Manager : MonoBehaviour
{
    public bool playOnAwake;
    public CutsceneList eventList;
    public GameObject mousePrompt;

    public AudioClip typingSound;
    
    private CutsceneState _currentState;
    private int _currentIndex;
    private CutsceneEvent _currentCutsceneEvent;

    private void Awake()
    {
        _currentState = CutsceneState.NotStarted;
        if(playOnAwake) StartCutscene();
    }

    private void StartCutscene()
    {
        UI_Manager.enableUI = false;
        _currentState = CutsceneState.Started;
        
        _currentIndex = 0;
        _currentCutsceneEvent = eventList.eventsList[_currentIndex];
        LoadCutsceneEvent();
    }

    private void LoadCutsceneEvent()
    {
        StopAllCoroutines();
        mousePrompt.SetActive(false);
        _currentState = CutsceneState.Playing;
        switch (_currentCutsceneEvent.cutsceneType)
        {
            case CutsceneType.Text:
                StartTypeText(_currentCutsceneEvent.tmp, _currentCutsceneEvent.message);
                break;
            case CutsceneType.Untype:
                StartUntypeText(_currentCutsceneEvent.tmp);
                break;
            case CutsceneType.Video:
                PlayAnimation(_currentCutsceneEvent.animator, _currentCutsceneEvent.triggerName);
                break;
        }
    }

    private void FinishCutscene()
    {
        _currentState = CutsceneState.Finished;
        eventList.onEndEvent?.Invoke();
        Player_StaticActions.EnableHumanMovement();
        Player_StaticActions.EnableDogMovement();
        UI_Manager.enableUI = true;
        Destroy(gameObject);
    }
    
    private void Update()
    {
        if (_currentState != CutsceneState.Waiting) return;

        if (Input.GetMouseButtonDown(0))
        {
           PlayNextEvent();
        }
    }

    private void PlayNextEvent()
    {
        _currentIndex++;
        if(_currentIndex == eventList.eventsList.Count) FinishCutscene();
        else
        {
            _currentCutsceneEvent = eventList.eventsList[_currentIndex];
            LoadCutsceneEvent();
        }
    }
    
    public void CutsceneEventFinished()
    {
        mousePrompt.SetActive(true);
        _currentState = CutsceneState.Waiting;
    }
    
    private void PlayAnimation(Animator animator, string triggerName)
    {
        if (_currentCutsceneEvent.isFirstFrame)
        {
            Color currentColor = _currentCutsceneEvent.image.color;
            currentColor.a = 1;
            _currentCutsceneEvent.image.color = currentColor;
        }
        animator.SetTrigger(triggerName);
    }
    
    private void StartTypeText(TextMeshProUGUI tmp, string message)
    {
        StartCoroutine(TypeText(tmp, message));
    }

    private void StartUntypeText(TextMeshProUGUI tmp)
    {
        StartCoroutine(UntypeText(tmp));
    }

    private IEnumerator TypeText(TextMeshProUGUI tmp, string message)
    {
        AudioManager.current.PlayLongSFX("Typewriter", typingSound);
        tmp.maxVisibleCharacters = 0;
        tmp.text = message;
        
        while(tmp.maxVisibleCharacters < tmp.text.Length)
        {
            tmp.maxVisibleCharacters++;
            yield return new WaitForSeconds(0.025f);
        }

        AudioManager.current.StopLongSFX("Typewriter");
        CutsceneEventFinished();
        StartCoroutine(TextLineFlash(tmp));
    }

    private IEnumerator UntypeText(TextMeshProUGUI tmp)
    {
        while (tmp.maxVisibleCharacters > 0)
        {
            tmp.maxVisibleCharacters--;
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.05f);
        CutsceneEventFinished();
        PlayNextEvent();
    }

    private IEnumerator TextLineFlash(TextMeshProUGUI tmp)
    {
        TMP_CharacterInfo charInfo = tmp.textInfo.characterInfo[tmp.text.Length - 1];
        TMP_MeshInfo[] meshInfo = tmp.textInfo.meshInfo;
        Color32[] meshColors = meshInfo[charInfo.materialReferenceIndex].colors32;
        int vertexIndex = charInfo.vertexIndex;

        while (true)
        {
            meshColors[vertexIndex + 0] = Color.clear;
            meshColors[vertexIndex + 1] = Color.clear;
            meshColors[vertexIndex + 2] = Color.clear;
            meshColors[vertexIndex + 3] = Color.clear;
            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            yield return new WaitForSeconds(0.2f);

            meshColors[vertexIndex + 0] = Color.white;
            meshColors[vertexIndex + 1] = Color.white;
            meshColors[vertexIndex + 2] = Color.white;
            meshColors[vertexIndex + 3] = Color.white;
            tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            yield return new WaitForSeconds(0.2f);
        }
    }
}

public enum CutsceneType
{
    Text,
    Untype,
    Video,
}

[Serializable]
public class CutsceneList
{
    public List<CutsceneEvent> eventsList;
    public UnityEvent onEndEvent;
}

[Serializable]
public class CutsceneEvent
{
    public CutsceneType cutsceneType;

    [Header("Text")]
    public TextMeshProUGUI tmp;
    [TextArea(3,5)]public string message;

    [Header("Video")]
    public Animator animator;
    public Image image;
    public bool isFirstFrame;
    public string triggerName;
}
