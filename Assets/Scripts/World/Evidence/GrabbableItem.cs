using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
    public InteractionIndicator indicator;
    public float radius;
    public Vector3 offsetPosition, offsetRotation;

    private Transform target;
    private DogController dog;
    private Vector3 origin, originRot;
    private bool grabbed = false;

    private void Awake()
    {
        origin = transform.position;
        originRot = transform.rotation.eulerAngles;
    }

    public void Drop()
    {
        grabbed = false;
        transform.parent = null;
        transform.position = origin;
        transform.rotation = Quaternion.Euler (originRot);
        indicator.Activate();
    }

    private void Update()
    {
        if (grabbed) return;

        if (Input.GetKeyDown (KeyCode.E) && F.FastDistance(transform.position, target.position) < radius * radius)
        {
            if (indicator) indicator.Deactivate();
            grabbed = true;
            dog.GrabItem(this);
        }
    }

    private void OnPlayersSpawned(GameObject human, GameObject dog)
    {
        target = dog.transform;
        this.dog = dog.GetComponentInParent<DogController>();
    }

    private void OnEnable()
    {
        EventManager.I.OnPlayersSpawned += OnPlayersSpawned;
    }

    private void OnDisable()
    {
        EventManager.I.OnPlayersSpawned -= OnPlayersSpawned;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
