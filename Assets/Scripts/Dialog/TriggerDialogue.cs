using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
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
    private Hashtable _hashtable = new Hashtable();

    private GameObject _humanGO;
    private GameObject _dogGO;
    private Vector3 _humanPosition;
    private Vector3 _dogPosition;

    private bool isInitialised;
    private bool isHumanPlayer;

    private void Awake()
    {
        _currentDialogueRunner = dialogCanvas.GetComponent<DialogueRunner>();
        _currentDialogueRunner.yarnProject = script;
        _currentDialogueRunner.VariableStorage = GameObject.FindGameObjectWithTag("YarnMemory").GetComponent<InMemoryVariableStorage>();
        _currentDialogueRunner.startNode = startNode;
        _npcCamera = transform.GetChild(0).GetComponent<Camera>();
       

        _currentDialogueRunner.onDialogueComplete.AddListener(delegate { ChangeCamera(true); });
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

        Invoke("SetupPlayers", 2f);
    }

    private void Update()
    {
        if (!isInitialised) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHumanPlayer && canBeTriggeredByHuman)
            {
                if (Vector3.Distance(_humanGO.transform.position, transform.position) < 5f)
                {
                    Player_StaticActions.DisableHumanMovement();
                    StartDialogue();
                }
            }
            else if (!isHumanPlayer && canBeTriggeredByDog)
            {
                _dogPosition = _dogGO.transform.position;

                if (Vector3.Distance(_dogGO.transform.position, transform.position) < 5f)
                {
                    Player_StaticActions.DisableDogMovement();
                    StartDialogue();
                }
            }
        }
        
    }

    private void SetupPlayers()
    {
        _playerCamera = Camera.main.GetComponent<Camera>();
        _humanGO = GameObject.FindGameObjectWithTag("HumanAgent");
        _dogGO = GameObject.FindGameObjectWithTag("DogAgent");
        isInitialised = true;
    }

    private void StartDialogue()
    {
        suspectSO.hasTalked = true;
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
        else
        {
            Player_StaticActions.EnableHumanMovement();
            Player_StaticActions.EnableDogMovement();
        }
    }
}
