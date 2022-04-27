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
}
