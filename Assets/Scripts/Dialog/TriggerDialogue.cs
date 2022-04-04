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
    
    private DialogueRunner _currentDialogueRunner;
    private bool _isHumanInRange;
    private bool _isDogInRange;

    private void Awake()
    {
        _currentDialogueRunner = dialogCanvas.GetComponent<DialogueRunner>();
        _currentDialogueRunner.yarnProject = script;
        _currentDialogueRunner.VariableStorage = GameObject.FindGameObjectWithTag("YarnMemory").GetComponent<InMemoryVariableStorage>();
        _currentDialogueRunner.startNode = startNode;
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
        dialogCanvas.SetActive(true);
    }

    public void EndDialogue()
    {
        dialogCanvas.SetActive(false);
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
