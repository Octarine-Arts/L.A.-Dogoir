using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SettingsManager : MonoBehaviourPunCallbacks
{
    public static SettingsManager current;
    
    public Slider masterVolSlider;
    public Slider BGMVolSlider;
    public Slider SFXVolSlider;

    private CanvasGroup _canvasGroup;
    
    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
        }
        else
        {
            current = this;
        }

        _canvasGroup = GetComponent<CanvasGroup>();
        masterVolSlider.onValueChanged.AddListener(delegate { SetMasterVol(); });
        BGMVolSlider.onValueChanged.AddListener(delegate { SetBGMVol(); });
        SFXVolSlider.onValueChanged.AddListener(delegate { SetSFXVol(); });
        
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0)) return;
        if (!UI_Manager.enableUI) return;
        
        if (!UI_Manager._isUIOpen)
        {
            if(Input.GetKeyDown(KeyCode.Escape)) OpenSettingsMenu();
        }
        else if (UI_Manager._isUIOpen && UI_Manager._currentMenu == "Settings")
        {
            if(Input.GetKeyDown(KeyCode.Escape)) CloseSettingsMenu();
        }
    }

    public void OpenSettingsMenu()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        UI_Manager.SetIsOpen(true, "Settings");
    }

    public void CloseSettingsMenu()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        UI_Manager.SetIsOpen(false, "Settings");
    }
    
    private void SetMasterVol()
    {
        AudioManager.current.SetMasterVolume(masterVolSlider.value);
    }

    private void SetBGMVol()
    {
        AudioManager.current.SetBGMVolume(BGMVolSlider.value);
    }

    private void SetSFXVol()
    {
        AudioManager.current.SetSFXVolume(SFXVolSlider.value);
    }

    public void ReturnToLobby()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        
        PhotonNetwork.LoadLevel(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
