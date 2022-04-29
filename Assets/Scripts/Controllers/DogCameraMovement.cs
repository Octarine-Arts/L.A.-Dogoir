using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogCameraMovement : MonoBehaviour
{
	public Transform target;
	public float hRotSpeed;
	public float vRotSpeed;
	public float followSpeed;

	private bool canMove = true; 
	private Vector3 offset;
	private float vRot;

	private void Awake ()
	{
		offset = transform.position - target.position;
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Locked;
    }

	private void Start()
	{
		Player_StaticActions.OnDisableDogMovement += DisableMovement;
		Player_StaticActions.OnEnableDogMovement += EnableMovement;
	}

	private void Update ()
	{
		if (!canMove) return;
		//Rotate camera
		Vector2 input = new Vector2 (Input.GetAxis ("Look X") * hRotSpeed, Input.GetAxis ("Look Y") * vRotSpeed);
		vRot = Mathf.Clamp (vRot - input.y, -30, 75);
		float hRot = transform.eulerAngles.y + input.x;

		transform.rotation = Quaternion.Euler (vRot, hRot, transform.rotation.eulerAngles.z);
		transform.position = Vector3.Lerp (transform.position, target.position + offset, followSpeed * Time.deltaTime);
	}

	private void FixedUpdate ()
	{
		//Follow target
		//transform.position = Vector3.Lerp (transform.position, target.position + offset, followSpeed * Time.deltaTime);
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
		Player_StaticActions.OnDisableDogMovement -= DisableMovement;
		Player_StaticActions.OnEnableDogMovement -= EnableMovement;
	}
}
