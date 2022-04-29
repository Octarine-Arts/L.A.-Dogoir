using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Journal;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class TriggerDialogue : MonoBehaviour
{
    public Suspect suspectSO;
    public GameObject dialogCanvas;
    public YarnProject script;
    public bool canBeTriggeredByHuman;
    public bool canBeTriggeredByDog;
    public string startNode;
    public UnityEvent onContinueEvent;
    
    private Camera _playerCamera;
    private Camera _npcCamera;
    private DialogueRunner _currentDialogueRunner;
    private CanvasGroup _canvasGroup;

    private GameObject _humanGO;
    private GameObject _dogGO;

    private bool isInitialised;
    private bool isTalking;

    private void Awake()
    {
        _currentDialogueRunner = dialogCanvas.GetComponent<DialogueRunner>();
        _currentDialogueRunner.yarnProject = script;
        _currentDialogueRunner.VariableStorage = GameObject.FindGameObjectWithTag("YarnMemory").GetComponent<InMemoryVariableStorage>();
        _currentDialogueRunner.startNode = startNode;
        _npcCamera = transform.GetChild(0).GetComponent<Camera>();
        _canvasGroup = dialogCanvas.GetComponent<CanvasGroup>();

        _currentDialogueRunner.onDialogueComplete.AddListener(delegate { ChangeCamera(true); });
        _npcCamera.enabled = false;
        HideCanvas();

        if (EventManager.I != null)
            EventManager.I.OnPlayersSpawned += OnPlayersSpawned;
    }

    private void Update()
    {
        if (!isInitialised) return;
        if (isTalking) return;
        if (UI_Manager._isUIOpen && UI_Manager._currentMenu == "NPC")
        {
            CheckContinuePressed();
        }
        else if (!UI_Manager._isUIOpen)
        {
            CheckOpenDialogue();
        }
    }

    private void CheckOpenDialogue()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerManager.ThisPlayer == PlayerSpecies.Human && canBeTriggeredByHuman)
            {
                
                if (Vector3.Distance(_humanGO.transform.position, transform.position) < 5f)
                {
                    Player_StaticActions.DisableHumanMovement();
                    StartDialogue();
                }
            }
            else if (PlayerManager.ThisPlayer == PlayerSpecies.Dog && canBeTriggeredByDog)
            {
                if (Vector3.Distance(_dogGO.transform.position, transform.position) < 5f)
                {
                    Player_StaticActions.DisableDogMovement();
                    StartDialogue();
                }
            }
        }
    }

    private void CheckContinuePressed()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            onContinueEvent.Invoke();
        }
    }
    
    private void OnPlayersSpawned (GameObject humanPlayer, GameObject dogPlayer)
    {
        _playerCamera = Camera.main.GetComponent<Camera> ();
        _humanGO = GameObject.FindGameObjectWithTag("HumanAgent");
        _dogGO = GameObject.FindGameObjectWithTag("DogAgent");
        isInitialised = true;

        Debug.Log(_humanGO);
    }

    private void ShowCanvas()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    private void HideCanvas()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    private void StartDialogue()
    {
        suspectSO.hasTalked = true;
        ChangeCamera(false);
    }

    public void EndDialogue()
    {
        _currentDialogueRunner.Stop();
        HideCanvas();
    }

    private void ChangeToNPCCamera()
    {
        isTalking = true;
        UI_Manager.SetIsOpen(true, "NPC");
        _playerCamera.enabled = false;
        _npcCamera.enabled = true;
    }
    
    private void ChangeToPlayerCamera()
    {
        isTalking = false;
        UI_Manager.SetIsOpen(false, "NPC");
        _playerCamera.enabled = true;
        _npcCamera.enabled = false;
    }

    public void ChangeCamera(bool changeToPlayer)
    {
        StartCoroutine(ChangeCamera_Coroutine(changeToPlayer));
    }
    
    private IEnumerator ChangeCamera_Coroutine(bool changeToPlayer)
    {
        ScreenFade.current.FadeToBlack();

        while (!ScreenFade.current.IsBlack())
        {
            yield return null;
        }

        if (changeToPlayer)
        {
            ChangeToPlayerCamera();
            EndDialogue();
        }
        else
        {
            ChangeToNPCCamera();
            ShowCanvas();
        }
        
        ScreenFade.current.FadeToTransparent();
        
        while (ScreenFade.current.IsBlack())
        {
            yield return null;
        }

        if (!changeToPlayer)
        {
            _currentDialogueRunner.StartDialogue(startNode);
        }
        else
        {
            Player_StaticActions.EnableHumanMovement();
            Player_StaticActions.EnableDogMovement();
        }
    }
}
