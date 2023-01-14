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
    public static Culprit_UI current;
    
    public GameObject cutscene;
    public CutsceneContentSetter cutsceneContentSetter;
    //public Cutscene_Manager cutsceneManager;
    
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
    public GameObject partnerAccuseText;
    
    private PhotonView _photonView;
    private CanvasGroup _canvasGroup;
    
    private int _currentSuspectIndex;
    private bool _isHumanConfirmed;
    private bool _isDogConfirmed;
    private bool _isOpen;
        
    private void Awake()
    {
        current = this;
        _currentSuspectIndex = 0;
        _photonView = GetComponent<PhotonView>();
        _canvasGroup = GetComponentInChildren<CanvasGroup>();

        EventManager.I.OnPlayersSpawned += PlayersSpawned;

        leftButton.onClick.AddListener(LeftButtonPressed);
        rightButton.onClick.AddListener(RightButtonPressed);
        humanButton.onClick.AddListener(ConfirmButtonPressed);
        dogButton.onClick.AddListener(ConfirmButtonPressed);
        
        UpdateUI();
        Close();
    }

    private void PlayersSpawned(GameObject human, GameObject dog)
    {
        if (PlayerManager.ThisPlayer == PlayerSpecies.Dog) humanButton.interactable = false;
        if (PlayerManager.ThisPlayer == PlayerSpecies.Human) dogButton.interactable = false;
        //gameObject.SetActive(false);
    }

    public void Open()
    {
        _isOpen = true;
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        UpdateUI();
    }

    public void Close()
    {
        _isOpen = false;
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Close();
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

        partnerAccuseText.SetActive(!_isOpen && 
            (_isDogConfirmed && PlayerManager.ThisPlayer == PlayerSpecies.Human ||
            _isHumanConfirmed && PlayerManager.ThisPlayer == PlayerSpecies.Dog));

        dogButton.image.sprite = _isDogConfirmed ? dogConfirmed : dogNotConfirmed;
        dogText.SetActive(_isDogConfirmed);

        humanButton.image.sprite = _isHumanConfirmed ? humanConfirmed : humanNotConfirmed;
        humanText.SetActive(_isHumanConfirmed);

        if (_isHumanConfirmed && _isDogConfirmed) SetEnding();
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
        //cutsceneManager.eventList.eventsList[^1].triggerName = endString;
        cutsceneContentSetter.SetContent(endString);
        cutsceneContentSetter.cutscene.StartCutscene();
        gameObject.SetActive(false);
    }
}
