using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerManager.ThisPlayer == PlayerSpecies.Human) Destroy(gameObject);
    }

    private void Update()
    {
        if (UI_Manager._isUIOpen) GetComponent<Canvas>().enabled = false;
        else if (!UI_Manager._isUIOpen) GetComponent<Canvas>().enabled = true;
    }
}
