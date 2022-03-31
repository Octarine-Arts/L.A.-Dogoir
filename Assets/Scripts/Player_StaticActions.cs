using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Player_StaticActions
{
    public static event Action OnDisableHumanMovement;
    public static void DisableHumanMovement()
    {
        OnDisableHumanMovement?.Invoke();
    }
    
    public static event Action OnEnableHumanMovement;
    public static void EnableHumanMovement()
    {
        OnEnableHumanMovement?.Invoke();
    }

    public static event Action OnDisableDogMovement;
    public static void DisableDogMovement()
    {
        OnDisableDogMovement?.Invoke();
    }

    public static event Action OnEnableDogMovement;
    public static void EnableDogMovement()
    {
        OnEnableDogMovement?.Invoke();
    }
}
