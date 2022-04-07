using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerManager.ThisPlayer == PlayerSpecies.Dog) Destroy(gameObject);
    }
}
