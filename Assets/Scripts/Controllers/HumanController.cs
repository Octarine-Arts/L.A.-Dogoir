using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class HumanController : MonoBehaviour
{
    public Animator anim;
    public Transform cam;
    public NavMeshAgent agent;
    public GameObject avatar;
    public Transform headLookTarget;
    public float rotateSpeed, moveAcceleration;

    public GameObject displayText;
    public GameObject humanCanvas;

    [SerializeField] private Vector2 verticalLookRange, lookSensitivity;

    private bool canMove = true;
    private PhotonView pview;
    private float pitch, currentSpeed;

    private void Awake ()
    {
        pview = GetComponent<PhotonView> ();
    }

    private void Start ()
    {
        print("HUMAN STARTED");

        if (EventManager.I != null)
        {
            print($"is avatar null? {avatar == null}, is eventmanagernull? {EventManager.I == null}");
            EventManager.I.PlayerSpawned (PlayerSpecies.Human, avatar);
        }

        print("Checking Human is Me? " + pview.IsMine);
        if (!pview.IsMine)
        {
            print("Destroying Human Stuff");
            Destroy (GetComponentInChildren<Camera> ().gameObject);
            Destroy (this);
            return;
        }
        else
        {
            foreach (SkinnedMeshRenderer renderer in avatar.GetComponentsInChildren<SkinnedMeshRenderer> ())
                renderer.enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Player_StaticActions.OnDisableHumanMovement += DisableMovement;
        Player_StaticActions.OnEnableHumanMovement += EnableMovement;
        Player_StaticActions.DisableHumanMovement();
    }

    private void Update ()
    {
        if (!canMove) return;
        Look ();
        Move ();
        PlayerRaycast();
    }

    private void Look ()
    {
        Vector2 lookInput = new Vector2 (Input.GetAxis ("Look X"), Input.GetAxis ("Look Y"));

        float yRot = cam.transform.rotation.eulerAngles.y + lookInput.x * lookSensitivity.x;
        pitch = Mathf.Clamp (pitch - lookInput.y * lookSensitivity.y, verticalLookRange.x, verticalLookRange.y);

        //Vector3 newRot = cam.transform.rotation.eulerAngles;
        //newRot.y = yRot;
        //newRot.x = pitch;

        cam.transform.localRotation = Quaternion.Euler (pitch, 0, 0);
        agent.transform.rotation = Quaternion.Euler (0, yRot, 0);
        headLookTarget.position = cam.transform.position + cam.transform.forward;
    }

    private void Move ()
    {
        if (UI_Manager._isUIOpen) return;
        
        Vector3 input = Quaternion.Euler (0, cam.rotation.eulerAngles.y, 0) * 
            new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));

        //print(input.sqrMagnitude);
        //currentSpeed = Mathf.Clamp01(currentSpeed + (input.sqrMagnitude > 0.1f ? moveAcceleration : -moveAcceleration) * Time.deltaTime);

        Vector3 smoothed = Vector3.Lerp(agent.destination - agent.transform.position, input.normalized, moveAcceleration * Time.deltaTime);

        agent.SetDestination(agent.transform.position + smoothed);//input.normalized * currentSpeed);

        anim.SetFloat ("MoveSpeed", Mathf.Clamp01 (agent.velocity.magnitude / (agent.speed / 3)));
    }

    private void PlayerRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 5f))
        {
            if (hit.collider.TryGetComponent<IInteractable> (out IInteractable interactable))
            {
                displayText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                    interactable.Interact();
            }
            else displayText.SetActive(false);
        }
    }

    private void EnableMovement()
    {
        canMove = true;
    }
    
    private void DisableMovement()
    {
        canMove = false;
    }

    private void OnDisable()
    {
        Player_StaticActions.OnDisableHumanMovement -= DisableMovement;
        Player_StaticActions.OnEnableHumanMovement -= EnableMovement;
    }
}
