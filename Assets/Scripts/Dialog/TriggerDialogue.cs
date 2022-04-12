using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Journal;

[RequireComponent(typeof(BoxCollider))]
public class TriggerDialogue : MonoBehaviour
{
    public Suspect suspectSO;
    public GameObject dialogCanvas;
    public YarnProject script;
    public bool canBeTriggeredByHuman;
    public bool canBeTriggeredByDog;
    public string startNode;

    private Camera _playerCamera;
    private Camera _npcCamera;
    private DialogueRunner _currentDialogueRunner;

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

        _currentDialogueRunner.onDialogueComplete.AddListener(delegate { ChangeCamera(true); });

        if (EventManager.I != null)
            EventManager.I.OnPlayersSpawned += OnPlayersSpawned;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
        
        if (!isInitialised) return;
        if (isTalking) return;

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

    private void OnPlayersSpawned (GameObject humanPlayer, GameObject dogPlayer)
    {
        _playerCamera = Camera.main.GetComponent<Camera> ();
        _humanGO = humanPlayer;
        _dogGO = dogPlayer;
        isInitialised = true;
    }

    private void StartDialogue()
    {
        //suspectSO.hasTalked = true;
        ChangeCamera(false);
    }

    public void EndDialogue()
    {
        dialogCanvas.SetActive(false);
    }

    private void ChangeToNPCCamera()
    {
        isTalking = true;
        Cursor.lockState = CursorLockMode.None;
        _playerCamera.enabled = false;
        _npcCamera.enabled = true;
    }
    
    private void ChangeToPlayerCamera()
    {
        isTalking = false;
        Cursor.lockState = CursorLockMode.Locked;
        _playerCamera.enabled = true;
        _npcCamera.enabled = false;
    }

    private void ChangeCamera(bool changeToPlayer)
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
            dialogCanvas.SetActive(true);
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
