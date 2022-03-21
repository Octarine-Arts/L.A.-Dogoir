using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class HumanController : MonoBehaviour
{
    public Transform cam;
    public NavMeshAgent agent;
    public MeshRenderer avatar;

    [SerializeField] private Vector2 verticalLookRange, lookSensitivity;

    private PhotonView pview;
    private float pitch;

    private void Awake ()
    {
        pview = GetComponent<PhotonView> ();
    }

    private void Start ()
    {
        if (!pview.IsMine)
        {
            Destroy (GetComponentInChildren<Camera> ().gameObject);
            Destroy (this);
            return;
        }
        else Destroy (avatar);
    }

    private void Update ()
    {
        Look ();
        Move ();
    }

    private void Look ()
    {
        Vector2 lookInput = new Vector2 (Input.GetAxis ("Look X"), Input.GetAxis ("Look Y"));

        float yRot = cam.transform.rotation.eulerAngles.y + lookInput.x * lookSensitivity.x;
        pitch = Mathf.Clamp (pitch - lookInput.y * lookSensitivity.y, verticalLookRange.x, verticalLookRange.y);

        Vector3 newRot = cam.transform.rotation.eulerAngles;
        newRot.y = yRot;
        newRot.x = pitch;

        cam.transform.rotation = Quaternion.Euler (newRot);
    }

    private void Move ()
    {
        Vector3 input = Quaternion.Euler (0, cam.rotation.eulerAngles.y, 0) * 
            new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));

        if (input.sqrMagnitude > 0.1f)
            agent.SetDestination (agent.transform.position + input.normalized * 0.25f);
    }
}
