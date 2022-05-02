using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class EndingUI : MonoBehaviour
{
    public static EndingUI current;
    
    public GameObject cutscene;
    public Cutscene_Manager cutsceneManager;

    private PhotonView _photonView;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        current = this;
        _photonView = GetComponent<PhotonView>();
        _canvasGroup = GetComponent<CanvasGroup>();
        Close();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public void Open()
    {
        UI_Manager.SetIsOpen(true, "End");
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    private void Close()
    {
        if (UI_Manager._isUIOpen && UI_Manager._currentMenu != "End") return;
            
        UI_Manager.SetIsOpen(false, "End");
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public void ChooseSuspect(string endString)
    {
        Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
        if (PlayerManager.ThisPlayer == PlayerSpecies.Human)
        {
            hashtable["HumanChosen"] = true;
            hashtable["HumanString"] = endString;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            ScreenFade.current.FadeToBlack();
        }
        else if (PlayerManager.ThisPlayer == PlayerSpecies.Dog)
        {
            hashtable["DogChosen"] = true;
            hashtable["DogString"] = endString;
            PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            ScreenFade.current.FadeToBlack();
        }

        SetEnding();
    }
    
    private void SetEnding()
    {
        Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;

        if ((bool) hashtable["DogChosen"] && (bool) hashtable["HumanChosen"])
        {
            string endString = "Fail";
            if((string) hashtable["HumanString"] == (string) hashtable["DogString"]) endString = (string) hashtable["HumanString"];

            _photonView.RPC(nameof(SetEnding_RPC), RpcTarget.AllBuffered, endString);
            Close();
        }
    }

    [PunRPC]
    private void SetEnding_RPC(string endString)
    {
        ScreenFade.current.FadeToTransparent();
        cutscene.SetActive(true);
        cutsceneManager.eventList.eventsList[^1].triggerName = endString;
        cutsceneManager.StartCutscene();
    }
}
