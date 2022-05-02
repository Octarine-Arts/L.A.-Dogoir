using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : MonoBehaviour
{
    public GameObject outerRing;
    public GameObject handle;

    private bool _canTurn;
    private Vector3 _screenPos;
    private Vector3 _angleOffset;

    private bool outerRingSpin;
    private bool handleSpin;
    
    private void Update()
    {
        if (!_canTurn) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            if (outerRingSpin)
            {
                _screenPos = Camera.main.WorldToScreenPoint(outerRing.transform.position);
                _angleOffset = outerRing.transform.localRotation.eulerAngles;
            }
            else if (handle)
            {
                _screenPos = Camera.main.WorldToScreenPoint(handle.transform.position);
                _angleOffset = handle.transform.localRotation.eulerAngles;
            }
            //Vector3 vec3 = Input.mousePosition - _screenPos;
            //_angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(vec3.y, -vec3.x)) * Mathf.Rad2Deg;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 vec3 = Input.mousePosition - _screenPos;
            float angle = Mathf.Atan2(vec3.y, -vec3.x) * Mathf.Rad2Deg;
            if(outerRingSpin) outerRing.transform.localEulerAngles =  _angleOffset + new Vector3(0, 0, angle);
            else if(handleSpin) handle.transform.localEulerAngles = _angleOffset + new Vector3(0, 0, angle);
        }
        if (Input.GetMouseButtonUp(0))
        {
            outerRingSpin = false;
            handleSpin = false;
            _canTurn = false;
        }
    }

    public void SetOuterRingSpin()
    {
        Debug.Log("Out");
        outerRingSpin = true;
        _canTurn = true;
    }

    public void SetHandleSpin()
    {
        Debug.Log("Handle");
        handleSpin = true;
        _canTurn = true;
    }
}
