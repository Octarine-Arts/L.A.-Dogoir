using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VaultPart
{
    OuterRing,
    Handle
}

public class VaultParts : MonoBehaviour
{
    public Vault vault;
    public VaultPart thisPart;

    public Camera _camera;



    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == name)
                {
                    if(thisPart == VaultPart.OuterRing) vault.SetOuterRingSpin();
                    else if(thisPart == VaultPart.Handle) vault.SetHandleSpin();
                }
            }
        }
    }
}
