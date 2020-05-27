 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    private float InputX, InputZ, Speed, gravity;
    private Camera cam;
    private CharacterController characterController;
	public float JumpForce;
	GameObject enemy;
	public bool move = false;
    public GameObject CameraSelected;
	public Rigidbody rb;
    private Vector3 desiredMoveDirection;
	public bool lockTarget = false;
	public bool AimCenter;
	[HideInInspector]public bool InBattle = false;
	[HideInInspector]public bool CanMove = true;

    [SerializeField] float rotationSpeed = 0.3f;
    [SerializeField] float allowRotation = 0.1f;
    public float movementSpeed = 1f;
    [SerializeField] float gravityMultipler;

    // Start is called before the first frame update
    void Start()
	{
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
        CameraSelected = GameObject.Find("CameraSelect");
		rb = GetComponent<Rigidbody> ();
    }

    // Update is called once per frame
    void Update()
	{
		JumpForce = GetComponent<jump> ().VerticalVelocity;
		InputX = Input.GetAxis ("Horizontal");
		InputZ = Input.GetAxis ("Vertical");
		Vector3 AimDirection = new Vector3 (GameObject.Find ("Center").transform.position.x, transform.position.y, GameObject.Find ("Center").transform.position.z);
		Vector3 LockDirection = new Vector3 (GameObject.Find ("ClosetEnemy").transform.position.x, transform.position.y, GameObject.Find ("ClosetEnemy").transform.position.z);
        InputDecider();
		if (InputX != 0) {
			move = true;
		}
		if (InputZ != 0){
			move = true;
		}
		if (InputX == 0 && InputZ == 0) {
			move = false;
		}
		if (CameraSelected.GetComponent<SelectCamera> ().CurrentCamera == GameObject.Find ("AimCenter")) {
			transform.LookAt (AimDirection);
		}
		if (lockTarget == true) {
			transform.LookAt (LockDirection);
		}
		if (InBattle == true) {
			GetComponent<Animator> ().SetBool ("Grounded",false);
			if (Input.GetKeyDown (KeyCode.Space) && GetComponent<Animator>().GetBool("Roll")!= true) {
				GetComponent<Animator> ().SetBool ("Roll", true);
			}
		}
		if (GetComponent<Animator> ().GetBool ("Hit") == true) {
			CanMove = false;
		}
		//if(IsDash == true){
			//transform.LookAt (AimDirection);
		//}
        MovementManager();
    }


    void InputDecider() 
    {
        Speed = new Vector2(InputX, InputZ).sqrMagnitude;

        if(Speed > allowRotation)
        {
            RotationManager();
        }
        else
        {
            desiredMoveDirection = Vector3.zero;
        }

    }



    void RotationManager()
    {
		Vector3 AimDirection = new Vector3 (GameObject.Find ("Center").transform.position.x, transform.position.y, GameObject.Find ("Center").transform.position.z);
		Vector3 LockDirection = new Vector3 (GameObject.Find ("ClosetEnemy").transform.position.x, transform.position.y, GameObject.Find ("ClosetEnemy").transform.position.z);

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();
		if (lockTarget == false || AimCenter == true) {
			desiredMoveDirection = forward * InputZ + right * InputX;
		} else {
			desiredMoveDirection = transform.forward * InputZ +transform.right * InputX;
		}

		if (AimCenter == true) {
			transform.LookAt (AimDirection);
			GetComponent<Animator> ().SetFloat ("InputX",InputX);
			GetComponent<Animator> ().SetFloat ("InputZ",InputZ);
			CameraSelected.GetComponent<SelectCamera> ().CurrentCamera = GameObject.Find ("AimCenter");
		} else if (lockTarget == true) {
			transform.LookAt (LockDirection);
			GetComponent<Animator> ().SetFloat ("InputX",InputX);
			GetComponent<Animator> ().SetFloat ("InputZ",InputZ);
		}
		else{
			if (CanMove == true) {
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), rotationSpeed);
			}
		}
    }

    void MovementManager()
    {
        Vector3 moveDirection = desiredMoveDirection * (movementSpeed * Time.deltaTime);
		moveDirection = new Vector3(moveDirection.x,JumpForce*Time.deltaTime, moveDirection.z);
		if (CanMove == true) {
			characterController.Move (moveDirection);
		}

    }
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		enemy = GameObject.FindWithTag ("Enemy");

		//if (hit.gameObject.tag == "Enemy") {
			//if (enemy.GetComponent<Enemy>().IsHit == true) {
				//if (isAttacked == false) {
					//isAttacked = true;
					//print ("IsHit");
					//GetComponent<Status> ().hp -= enemy.GetComponent<simpleEnemy> ().Damage;
				//}
			//}
		//}
	}
	public void StartRoll(){
		GetComponent<Status> ().Invincible = true;
		GetComponent<Animator> ().applyRootMotion = true;
		GetComponent<Animator> ().SetBool ("Hit",false);
	}

	public void endRoll(){
		print ("End Roll");
		GetComponent<Status> ().Invincible = false;
		GetComponent<Animator> ().SetBool ("Roll", false);
		if (GetComponent<ChangeAnimation> ().CurrentAnimation == ChangeAnimation.Animation.Idle) {
			GetComponent<Animator> ().SetBool ("Idle", true);
		}
		if (GetComponent<ChangeAnimation> ().CurrentAnimation == ChangeAnimation.Animation.Walk) {
			GetComponent<Animator> ().SetBool ("Walk", true);
		}
		if (GetComponent<ChangeAnimation> ().CurrentAnimation == ChangeAnimation.Animation.Run) {
			GetComponent<Animator> ().SetBool ("Run", true);
		}
		GetComponent<Animator> ().applyRootMotion = false;
	}
	public void Destroy(){
		Destroy (this.gameObject);
	}
    public void ChangeMove(bool x)
    {
        CanMove = x;
    }
}


