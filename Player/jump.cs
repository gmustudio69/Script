using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour {
	private CharacterController controller;
	public bool isJump = false;
	public float jumpForce = 10.0f;
	public float VerticalVelocity;
	private float gravity = 14.0f;
	public GameObject player;
	public float disToGround = 0.5f;
	public LayerMask ground;
	public MovementController MovementScript;
	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
		MovementScript = GetComponent<MovementController> ();
	}
	
	// Update is called once per frame
	void Update () {
			if (controller.isGrounded == true) {
				VerticalVelocity = -gravity * Time.deltaTime;
				if (MovementScript.InBattle == false){
					if (Input.GetKeyDown (KeyCode.Space)) {
						VerticalVelocity = jumpForce;
					}
				}
			} else {
				VerticalVelocity -= gravity * Time.deltaTime;
			}
		//Vector3 MoveDirection = new Vector3 (0,VerticalVelocity,0);
		//controller.Move (MoveDirection * Time.deltaTime);


	}
}

