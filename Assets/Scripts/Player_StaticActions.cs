using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Player_StaticActions
{
    public static bool humanMovementAllowed;
    public static bool dogMovementAllowed;
    
    public static event Action OnDisableHumanMovement;
    public static void DisableHumanMovement()
    {
        humanMovementAllowed = false;
        OnDisableHumanMovement?.Invoke();
    }
    
    public static event Action OnEnableHumanMovement;
    public static void EnableHumanMovement()
    {
        humanMovementAllowed = true;
        OnEnableHumanMovement?.Invoke();
    }

    public static event Action OnDisableDogMovement;
    public static void DisableDogMovement()
    {
        dogMovementAllowed = false;
        OnDisableDogMovement?.Invoke();
    }

    public static event Action OnEnableDogMovement;
    public static void EnableDogMovement()
    {
        dogMovementAllowed = true;
        OnEnableDogMovement?.Invoke();
    }
}
