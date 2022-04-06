using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(BoxCollider))]
public class TriggerDialogue : MonoBehaviour
{
    public GameObject dialogCanvas;
    public YarnProject script;
    public bool canBeTriggeredByHuman;
    public bool canBeTriggeredByDog;
    public string startNode;

    private Camera _playerCamera;
    private Camera _npcCamera;
    private DialogueRunner _currentDialogueRunner;
    private bool _isHumanInRange;
    private bool _isDogInRange;
    private Hashtable _hashtable = new Hashtable();

    private bool isHumanPlayer;

    private void Awake()
    {
        _currentDialogueRunner = dialogCanvas.GetComponent<DialogueRunner>();
        _currentDialogueRunner.yarnProject = script;
        _currentDialogueRunner.VariableStorage = GameObject.FindGameObjectWithTag("YarnMemory").GetComponent<InMemoryVariableStorage>();
        _currentDialogueRunner.startNode = startNode;
        _npcCamera = transform.GetChild(0).GetComponent<Camera>();
        _playerCamera = Camera.main.GetComponent<Camera>();

        _currentDialogueRunner.onDialogueComplete.AddListener(delegate { ChangeCamera(true);  });
        _hashtable = PhotonNetwork.CurrentRoom.CustomProperties;

        if (PhotonNetwork.IsMasterClient)
        {
            if ((int)_hashtable["HSlot"] == 1)
            {
                isHumanPlayer = true;
            }
            else
            {
                isHumanPlayer = false;
            }
        }
        else
        {
            if ((int)_hashtable["GSlot"] == 1)
            {
                isHumanPlayer = true;
            }
            else
            {
                isHumanPlayer = false;
            }
        }

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _isHumanInRange && canBeTriggeredByHuman && isHumanPlayer)
        {
            Player_StaticActions.DisableHumanMovement();
            StartDialogue();
        }

        if (Input.GetKeyDown(KeyCode.E) && _isDogInRange && canBeTriggeredByDog && !isHumanPlayer)
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
        _playerCamera.enabled = false;
        _npcCamera.enabled = true;
    }
    
    private void ChangeToPlayerCamera()
    {
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
