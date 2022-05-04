using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class DogController : MonoBehaviour
{
    public Animator anim;
    public NavMeshAgent dog;
    public Transform camPivot, grabItemParent;
    public float rotateSpeed;

    private bool canMove = true; 
    private PhotonView pview;
    private float speed;
    private GrabbableItem grabbedItem;

    private void Awake ()
    {
        pview = GetComponent<PhotonView> ();
        speed = dog.speed;
    }

    private void Start ()
    {
        print("DOG STARTED");
        if (EventManager.I != null)
            EventManager.I.PlayerSpawned (PlayerSpecies.Dog, dog.gameObject);

        print("Checking Dog is Me? " + pview.IsMine);
        if (!pview.IsMine)
        {
            print("Destroying Dog Stuff");
            Destroy(GetComponentInChildren<Camera> ().gameObject);
            Destroy(dog);
            Destroy (this);
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Player_StaticActions.OnDisableDogMovement += DisableMovement;
        Player_StaticActions.OnEnableDogMovement += EnableMovement;
        Player_StaticActions.DisableDogMovement();
        
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3))
        {
            dog.transform.localScale *= 1.5f;
            transform.GetChild(0).GetChild(0).localPosition += Vector3.back;
        }
    }


    public void GrabItem(Transform item) => pview.RPC("GrabItem_RPC", RpcTarget.All, item.GetComponent<GrabbableItem> ());
    [PunRPC]
    private void GrabItem_RPC(GrabbableItem item)
    {
        if (grabbedItem) grabbedItem.Drop();

        grabbedItem = item;
        item.transform.parent = grabItemParent;
        item.transform.localPosition = item.offsetPosition;
        item.transform.localRotation = Quaternion.Euler (item.offsetRotation);
        item.transform.localScale *= item.scaleMultiplier;
    }

    public void DropItem(bool disable = false) => pview.RPC("DropItem_RPC", RpcTarget.All, disable);
    [PunRPC]
    public void DropItem_RPC(bool disable = false)
    {
        if (disable) grabbedItem.gameObject.SetActive(false);
        else grabbedItem.Drop();

        grabbedItem = null;
    }

    private void Update ()
    {
        if (!canMove) return;
        if (UI_Manager._isUIOpen) return;
        
        Vector3 input = Quaternion.Euler (0, camPivot.rotation.eulerAngles.y, 0) * 
            new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));

        float angleDif = (1 - (Vector3.Angle (input, dog.transform.forward) / 180f));

        if (input.magnitude > 0.1f)
        {
            print("velocity thresh reached");
            Quaternion targetRot = Quaternion.Euler (dog.transform.rotation.eulerAngles.x,
                input.Angle(),
                dog.transform.rotation.eulerAngles.z);

            dog.transform.rotation = Quaternion.Lerp (dog.transform.rotation, targetRot, Time.deltaTime * rotateSpeed * (angleDif + 0.2f) / 1.2f);
        }

        dog.speed = Mathf.Lerp (0.1f, speed, angleDif);

        dog.SetDestination (dog.transform.position + dog.transform.forward * input.magnitude);
        anim.SetFloat ("MoveSpeed", dog.velocity.magnitude);
    }

    private void EnableMovement()
    {
        canMove = true;
    }

    private void DisableMovement()
    {
        canMove = false;
        dog.SetDestination(dog.transform.position);
        anim.SetFloat("MoveSpeed", 0);
    }

    public void SetSniffing(bool sniffing) => anim.SetBool("Sniffing", sniffing);

    private void OnDisable()
    {
        Player_StaticActions.OnDisableDogMovement -= DisableMovement;
        Player_StaticActions.OnEnableDogMovement -= EnableMovement;
    }
}
