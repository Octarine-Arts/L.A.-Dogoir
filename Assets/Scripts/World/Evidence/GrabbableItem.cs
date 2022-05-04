using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
    public InteractionIndicator indicator;
    public float radius;
    public Vector3 offsetPosition, offsetRotation;
    public float scaleMultiplier = 1f;
    public string yarnBool;

    private Transform target;
    private DogController dog;
    private Vector3 origin, originRot, originScale;
    private bool grabbed = false;

    private void Awake()
    {
        if (yarnBool[0] != '$') yarnBool = "$" + yarnBool;

        origin = transform.position;
        originRot = transform.rotation.eulerAngles;
        originScale = transform.localScale;
    }

    public void Drop()
    {
        grabbed = false;
        transform.parent = null;
        transform.position = origin;
        transform.rotation = Quaternion.Euler (originRot);
        transform.localScale = originScale;
        indicator.Activate();
        if (!string.IsNullOrEmpty(yarnBool))
            YarnCommands.current.SetBool(yarnBool, false);
    }

    private void Grab()
    {
        if (indicator) indicator.Deactivate();
        grabbed = true;
        dog.GrabItem(this);
        if (!string.IsNullOrEmpty (yarnBool))
            YarnCommands.current.SetBool(yarnBool, true);
    }

    private void Update()
    {
        if (grabbed) return;

        if (Input.GetKeyDown (KeyCode.E) && F.FastDistance(transform.position, target.position) < radius * radius)
            Grab();
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
