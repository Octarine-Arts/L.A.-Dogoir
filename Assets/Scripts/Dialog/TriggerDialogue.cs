using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TriggerDialogue : MonoBehaviour
{
    public GameObject dialogCanvas;
    public YarnProject script;
    public bool canBeTriggeredByHuman;
    public bool canBeTriggeredByDog;
    public string startNode;

    private GameObject _playerCamera;
    private GameObject _npcCamera;
    private DialogueRunner _currentDialogueRunner;
    private bool _isHumanInRange;
    private bool _isDogInRange;

    private void Awake()
    {
        _currentDialogueRunner = dialogCanvas.GetComponent<DialogueRunner>();
        _currentDialogueRunner.yarnProject = script;
        _currentDialogueRunner.VariableStorage = GameObject.FindGameObjectWithTag("YarnMemory").GetComponent<InMemoryVariableStorage>();
        _currentDialogueRunner.startNode = startNode;
        _npcCamera = transform.GetChild(0).gameObject;
        _playerCamera = Camera.main.gameObject;

        _currentDialogueRunner.onDialogueComplete.AddListener(delegate { ChangeCamera(true);  });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
        
        if (Input.GetKeyDown(KeyCode.E) && _isHumanInRange && canBeTriggeredByHuman)
        {
            Player_StaticActions.DisableHumanMovement();
            StartDialogue();
        }

        if (Input.GetKeyDown(KeyCode.E) && _isDogInRange && canBeTriggeredByDog)
        {
            Player_StaticActions.DisableDogMovement();
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        ChangeCamera(false);
    }

    public void EndDialogue()
    {
        dialogCanvas.SetActive(false);
    }

    private void ChangeToNPCCamera()
    {
        _playerCamera.SetActive(false);
        _npcCamera.SetActive(true);
    }
    
    private void ChangeToPlayerCamera()
    {
        _playerCamera.SetActive(true);
        _npcCamera.SetActive(false);
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
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HumanPlayer"))
        {
            _isHumanInRange = true;
        }
        else if (other.CompareTag("DogPlayer"))
        {
            _isDogInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HumanPlayer"))
        {
            _isHumanInRange = false;
        }
        else if (other.CompareTag("DogPlayer"))
        {
            _isDogInRange = false;
        }
    }
}
