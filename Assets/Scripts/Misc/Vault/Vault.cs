using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Vault : MonoBehaviour, IInteractable
{
    public GameObject outerRing;
    public GameObject handle;

    public Camera _camera;
    public string password;

    public CanvasGroup _canvasGroup;
    
    public GameObject passwordParent;
    public TMP_Text letter0;
    public TMP_Text letter1;
    public TMP_Text letter2;
    public TMP_Text letter3;

    private bool _hasOpened;
    private bool _canTurn;
    private Vector3 _screenPos;
    private float _angleOffset;

    private Camera _playerCamera;
    private string currentLetter;
    private string currentPassword = "";

    private bool outerRingSpin;
    private bool handleSpin;

    private void Awake()
    {
        if (EventManager.I != null) EventManager.I.OnPlayersSpawned += OnPlayersSpawned;
    }
    
    private void OnPlayersSpawned (GameObject humanPlayer, GameObject dogPlayer)
    {
        _playerCamera = Camera.main.GetComponent<Camera> ();
    }
    
    private void Update()
    {
        if (UI_Manager._isUIOpen && UI_Manager._currentMenu == "Vault")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ChangeCamera(true);
            }
        }
        
        if (!_canTurn) return;
        if (Input.GetMouseButton(0))
        {
            Vector3 vec3 = Input.mousePosition - _screenPos;
            float angle = Mathf.Atan2(vec3.y, -vec3.x) * Mathf.Rad2Deg;
            if(outerRingSpin) outerRing.transform.localEulerAngles = new Vector3(0, 0, angle + _angleOffset);
            else if(handleSpin) handle.transform.localEulerAngles =  new Vector3(0, 0, angle + _angleOffset);
        }
        if (Input.GetMouseButtonUp(0))
        {
            outerRingSpin = false;
            handleSpin = false;
            _canTurn = false;
            UpdatePassword();
        }
    }

    private void UpdatePassword()
    {
        if (currentPassword.Length >= 4) currentPassword = "";
        
        currentPassword += currentLetter;

        letter0.text = currentPassword.Length >= 1 ? "" + currentPassword[0] : "";
        letter1.text = currentPassword.Length >= 2 ? "" + currentPassword[1] : "";
        letter2.text = currentPassword.Length >= 3 ? "" + currentPassword[2] : "";
        letter3.text = currentPassword.Length >= 4 ? "" + currentPassword[3] : "";

        if (password == currentPassword)
        {
            _hasOpened = true;
            passwordParent.SetActive(false);
            GetComponent<Animator>().SetTrigger("Open");
            ChangeCamera(true);
        }
    }

    public void SetCurrentLetter(string letter)
    {
        currentLetter = letter;
    }

    public void SetOuterRingSpin()
    {
        outerRingSpin = true;
        _canTurn = true;
        
        _screenPos = _camera.WorldToScreenPoint(outerRing.transform.position);
        Vector3 vec3 = Input.mousePosition - _screenPos;
        _angleOffset = (Mathf.Atan2(outerRing.transform.right.y, -outerRing.transform.right.x) - Mathf.Atan2(vec3.y, -vec3.x)) * Mathf.Rad2Deg;
    }

    public void SetHandleSpin()
    {
        handleSpin = true;
        _canTurn = true;
        
        _screenPos = _camera.WorldToScreenPoint(handle.transform.position);
        Vector3 vec3 = Input.mousePosition - _screenPos;
        _angleOffset = (Mathf.Atan2(handle.transform.right.y, -outerRing.transform.right.x) - Mathf.Atan2(vec3.y, -vec3.x)) * Mathf.Rad2Deg;
    }

	public void Interact()
	{
		ChangeCamera(false);
	}
 
	public bool CanInteract()
    {
        return !_hasOpened;
    }
    
    private void ShowCanvas()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }
 
    private void HideCanvas()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
 
    private void StartVault()
    {
        UI_Manager.SetIsOpen(true, "Vault");
        GetComponent<BoxCollider>().enabled = false;
        ShowCanvas();
    }
 
    public void EndVault()
    {
        UI_Manager.SetIsOpen(false, "Vault");
        if(!_hasOpened) GetComponent<BoxCollider>().enabled = true;
        HideCanvas();
    }
 
    private void ChangeToNPCCamera()
    {
        _playerCamera.enabled = false;
        _camera.enabled = true;
    }
    
    private void ChangeToPlayerCamera()
    {
        _playerCamera.enabled = true;
        _camera.enabled = false;
    }
 
    public void ChangeCamera(bool changeToPlayer)
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
            EndVault();
        }
        else
        {
            ChangeToNPCCamera();
            ShowCanvas();
        }
        
        ScreenFade.current.FadeToTransparent();
        
        while (ScreenFade.current.IsBlack())
        {
            yield return null;
        }
 
        if (!changeToPlayer)
        {
            StartVault();
        }
        else
        {
            Player_StaticActions.EnableHumanMovement();
            Player_StaticActions.EnableDogMovement();
        }
    }
}
