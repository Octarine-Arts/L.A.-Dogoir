using System;
using System.Collections;
using System.Collections.Generic;
using Journal;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Culprit_UI : MonoBehaviour
{
    public GameObject cutscene;
    public Cutscene_Manager cutsceneManager;
    
    public Image suspectImage;
    public List<Suspect> suspectList;

    public Button leftButton;
    public Button rightButton;
    public Button humanButton;
    public Button dogButton;

    public Sprite humanNotConfirmed;
    public Sprite dogNotConfirmed;
    public Sprite humanConfirmed;
    public Sprite dogConfirmed;
    public GameObject humanText;
    public GameObject dogText;
    
    private PhotonView _photonView;
    
    private int _currentSuspectIndex;
    private bool _isHumanConfirmed;
    private bool _isDogConfirmed;
        
    private void Awake()
    {
        _currentSuspectIndex = 0;
        _photonView = GetComponent<PhotonView>();

        EventManager.I.OnPlayersSpawned += PlayersSpawned;

        leftButton.onClick.AddListener(LeftButtonPressed);
        rightButton.onClick.AddListener(RightButtonPressed);
        humanButton.onClick.AddListener(ConfirmButtonPressed);
        dogButton.onClick.AddListener(ConfirmButtonPressed);
        
        UpdateUI();
    }

    private void PlayersSpawned(GameObject human, GameObject dog)
    {
        if (PlayerManager.ThisPlayer == PlayerSpecies.Dog) humanButton.interactable = false;
        if (PlayerManager.ThisPlayer == PlayerSpecies.Human) dogButton.interactable = false;
        //gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) gameObject.SetActive(false);
    }

    private void LeftButtonPressed()
    {
        _photonView.RPC(nameof(PreviousSuspect_RPC), RpcTarget.AllBuffered);
        _photonView.RPC(nameof(ResetConfirms_RPC), RpcTarget.AllBuffered);
    }

    private void RightButtonPressed()
    {
        _photonView.RPC(nameof(NextSuspect_RPC), RpcTarget.AllBuffered);   
        _photonView.RPC(nameof(ResetConfirms_RPC), RpcTarget.AllBuffered);
    }

    private void ConfirmButtonPressed()
    {
        _photonView.RPC(nameof(ConfirmPressed_RPC), RpcTarget.AllBuffered, PlayerManager.ThisPlayer);   
    }

    [PunRPC]
    private void PreviousSuspect_RPC()
    {
        _currentSuspectIndex--;
        if (_currentSuspectIndex < 0) _currentSuspectIndex = suspectList.Count - 1;
        UpdateUI();
    }

    [PunRPC]
    private void NextSuspect_RPC()
    {
        _currentSuspectIndex++;
        if (_currentSuspectIndex >= suspectList.Count) _currentSuspectIndex = 0;
        UpdateUI();
    }

    [PunRPC]
    private void ConfirmPressed_RPC(PlayerSpecies player)
    {
        if (player == PlayerSpecies.Dog)
        {
            _isDogConfirmed = !_isDogConfirmed;
        }
        else if (player == PlayerSpecies.Human)
        {
            _isHumanConfirmed = !_isHumanConfirmed;
        }
        
        UpdateUI();
    }

    [PunRPC]
    private void ResetConfirms_RPC()
    {
        _isDogConfirmed = false;
        _isHumanConfirmed = false;
        
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        suspectImage.sprite = suspectList[_currentSuspectIndex].displayImage;

        if (_isDogConfirmed)
        {
            dogButton.image.sprite = dogConfirmed;
            dogText.SetActive(true);
        }
        else
        {
            dogButton.image.sprite = dogNotConfirmed;
            dogText.SetActive(false);
        }

        if (_isHumanConfirmed)
        {
            humanButton.image.sprite = humanConfirmed;
            humanText.SetActive(true);
        }
        else
        {
            humanButton.image.sprite = humanNotConfirmed;
            humanText.SetActive(false);
        }
        
        if(_isHumanConfirmed && _isDogConfirmed) SetEnding();
    }
    
    private void SetEnding()
    {
        Suspect culprit = suspectList[_currentSuspectIndex];

        string endString = "";
        switch (culprit.fullName)
        {
            case "Dale Butterbur":
                endString = "Dale";
                break;
            case "Jack Harlow":
                endString = "Jack";
                break;
            case "Loretta Dubois":
                endString = "Loretta";
                break;
            case "Peggy Harlow":
                endString = "Peggy";
                break;
            case "Johnny Malone":
                endString = "Johnny";
                break;
        }

        SetEnding_RPC(endString);
        //_photonView.RPC(nameof(SetEnding_RPC), RpcTarget.AllBuffered, endString);
    }

    [PunRPC]
    private void SetEnding_RPC(string endString)
    {
        ScreenFade.current.FadeToTransparent();
        cutscene.SetActive(true);
        cutsceneManager.eventList.eventsList[^1].triggerName = endString;
        cutsceneManager.StartCutscene();
        gameObject.SetActive(false);
    }
}
