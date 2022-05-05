using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour
{
    public List<GameObject> goToEnable;
    
    private PhotonView _photonView;
    private bool _isTransitioning;
    
    private bool isEnabled;
    private bool dogPresent;
    private bool humanPresent;

    private GameObject _humanGO;
    private GameObject _dogGO;
    
    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        EventManager.I.OnPlayersSpawned += Setup;
    }

    private void OnDisable()
    {
        EventManager.I.OnPlayersSpawned -= Setup;
    }

    private void Setup(GameObject humanGO, GameObject dogGO)
    {
        _humanGO = GameObject.FindGameObjectWithTag("HumanAgent");
        _dogGO = GameObject.FindGameObjectWithTag("DogAgent");
        
        Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
        hashtable["DogReady"] = false;
        hashtable["HumanReady"] = false;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        
        foreach(GameObject go in goToEnable) go.SetActive(false);
    }
    
    private void Update()
    {
        if (!isEnabled) return;

        humanPresent = Vector3.Distance(_humanGO.transform.position, transform.position) < 2f;
        dogPresent = Vector3.Distance(_dogGO.transform.position, transform.position) < 2f;
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pressed");
            if (dogPresent && PlayerManager.ThisPlayer == PlayerSpecies.Dog)
            {
                Debug.Log("Pressed Dog");
                Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
                hashtable["DogReady"] = true;
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            }
            else if (humanPresent && PlayerManager.ThisPlayer == PlayerSpecies.Human)
            {
                Debug.Log("Pressed Human");
                Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
                hashtable["HumanReady"] = true;
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
            }

            if(!_isTransitioning) CheckTransition();
        }
    }

    public void SetEnabled()
    {
        _photonView.RPC(nameof(SetEnabled_RPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void SetEnabled_RPC()
    {
        foreach(GameObject go in goToEnable) go.SetActive(true);
        isEnabled = true;
    }
    
    private void CheckTransition()
    {
        Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
        if((bool) hashtable["HumanReady"] && (bool) hashtable["DogReady"])
        {
            _isTransitioning = true;
            int index = SceneManager.GetSceneByName("Bar_Scene").buildIndex;
            if (PhotonNetwork.IsMasterClient)
            {
                ScreenFade.current.FadeToBlack();
                PhotonNetwork.LoadLevel(index);
            }
            else
            {
                _photonView.RPC(nameof(LoadLevel_RPC), RpcTarget.MasterClient);
            }
        }
    }

    [PunRPC]
    private void LoadLevel_RPC()
    {
        ScreenFade.current.FadeToBlack();
        _isTransitioning = true;
        int index = SceneManager.GetSceneByName("Bar_Scene").buildIndex;
        PhotonNetwork.LoadLevel(index);
    }
}
