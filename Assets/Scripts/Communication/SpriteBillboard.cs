using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField] private bool backwards;
    private Transform cam;

    private void Start ()
    {
        print (Camera.main);
        cam = Camera.main.transform;
    }

    private void Update ()
    {
        //transform.LookAt (backwards ? transform.position - cam.position : cam.position);
        transform.forward = backwards ? cam.forward : -cam.forward;
    }
}
