using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockParts : MonoBehaviour
{
    public Vault vault;
    public LayerMask layers;

    // Update is called once per frame
    void Update()
    {
        Ray();
    }

    private void Ray()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.up, out hitInfo, 5f, layers))
        {
            Debug.Log(name);
            vault.SetCurrentLetter(name);
        }
    }
}
