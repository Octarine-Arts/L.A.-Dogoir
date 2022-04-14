using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Serialization;

public class HumanController2 : MonoBehaviour
{
    public Animator anim;
    public Transform cam;
    public GameObject agent;
    public Transform headLookTarget;
    public float moveSpeed;
    
    public GameObject displayText;
    public GameObject humanCanvas;

    [SerializeField] private Vector2 verticalLookRange, lookSensitivity;

    private Rigidbody avatarRB;
    private bool canMove = true;
    private PhotonView pview;
    private float pitch;

    private void Awake()
    {
        pview = GetComponent<PhotonView>();
        avatarRB = agent.GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (EventManager.I != null) EventManager.I.PlayerSpawned (PlayerSpecies.Human, agent);

        if (!pview.IsMine)
        {
            Destroy (GetComponentInChildren<Camera> ().gameObject);
            Destroy (this);
            return;
        }

        foreach (SkinnedMeshRenderer renderer in agent.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            renderer.enabled = false;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Player_StaticActions.OnDisableHumanMovement += DisableMovement;
        Player_StaticActions.OnEnableHumanMovement += EnableMovement;
    }

    // Update is called once per frame
    void Update()
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

        cam.transform.localRotation = Quaternion.Euler (pitch, 0, 0);
        agent.transform.rotation = Quaternion.Euler (0, yRot, 0);
        headLookTarget.position = cam.transform.position + cam.transform.forward;
    }

    private void Move ()
    {
        Vector3 input = Quaternion.Euler (0, cam.rotation.eulerAngles.y, 0) * 
                        new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));

        avatarRB.velocity = input.normalized * moveSpeed;
        anim.SetFloat ("MoveSpeed", Mathf.Clamp01(avatarRB.velocity.x));
    }
    
    private void PlayerRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, 5f))
        {
            if (hit.collider.TryGetComponent (out IInteractable interactable))
            {
                displayText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E)) interactable.Interact();
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
}
