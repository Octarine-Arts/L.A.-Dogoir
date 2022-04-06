using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    private void Awake ()
    {
        if (Instance != null)
        {
            Destroy (gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad (gameObject);
    }

    public override void OnEnable ()
    {
        base.OnEnable ();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable ()
    {
        base.OnDisable ();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    
    private void OnSceneLoaded (Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 0) return;

        PhotonNetwork.Instantiate (Path.Combine ("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
    }
}
