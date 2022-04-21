using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Photon.Pun;

public class YarnCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cornelius"))
        {
            YarnCommands.current.SetBool("$CorneliusIsBroughtToDan", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cornelius"))
        {
            YarnCommands.current.SetBool("$CorneliusIsBroughtToDan", false);
        }
    }
}
