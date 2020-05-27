using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAnimation : MonoBehaviour
{
	private Animator anim;
	private MovementController Movement;
	private jump Jumpment;
	[HideInInspector]public bool Idle, Walk,Jump,Attack,Run,Kick,Block;
	[HideInInspector]public enum Animation{ None,Idle, Walk,Jump,land,Attack,Run,Kick};
	[HideInInspector]public  Animation CurrentAnimation = Animation.Idle;
	public CharacterController Controller;
	public BasicAttack AttackBase;
	[Header("Animator")]
	public RuntimeAnimatorController Base,UnArmNonLock,UnArm,KatanaNonLock,Katana,ThrowKnife;
	public Status PlayerStatus;
    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator> ();
		Movement = GetComponent<MovementController> ();
		Controller = GetComponent<CharacterController> ();
		AttackBase = GetComponent<BasicAttack> ();
		PlayerStatus = GetComponent<Status> ();
	}

    // Update is called once per frame
    void Update()
    {
		if (this.gameObject.name == "Player") {
			PlayerAnimation ();

		}
		anim.SetBool ("Grounded", Controller.isGrounded);
		if (anim.GetBool ("Attack1") == true) {
			Movement.CanMove = false;
		}
    }
	void PlayerAnimation(){
		if (PlayerStatus.IsProjective == true) {
			anim.runtimeAnimatorController = ThrowKnife;
		}
		else{
		if (Movement.InBattle == false) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				CurrentAnimation = Animation.Jump;
			}
			if (anim.runtimeAnimatorController != Base) {
				anim.runtimeAnimatorController = Base;
			}
		} if (Movement.InBattle == true){
			if (Movement.lockTarget == false) {
				if (CurrentAnimation == Animation.Jump) {
					CurrentAnimation = Animation.Idle;
				}
				if (PlayerStatus.CurrentWeapon == Status.AllWeapon.Arm) {
					anim.runtimeAnimatorController = UnArmNonLock;
				}
				if (PlayerStatus.CurrentWeapon == Status.AllWeapon.Katana) {
					anim.runtimeAnimatorController = KatanaNonLock;
				}
			} else {
				if (PlayerStatus.CurrentWeapon == Status.AllWeapon.Arm) {
					anim.runtimeAnimatorController = UnArm;
				}
				if (PlayerStatus.CurrentWeapon == Status.AllWeapon.Katana) {
					anim.runtimeAnimatorController = Katana;
				}
			}
			if (Input.GetKeyDown (KeyCode.F)) {
				Kick = true;
			}
			anim.SetBool ("Block",Block);
		}
		if (CurrentAnimation != Animation.Jump) {
			if (Movement.move == true) {
					CurrentAnimation = Animation.Walk;
			} else {
					CurrentAnimation = Animation.Idle;
			}
			if (Input.GetMouseButtonDown (0)) {
				CurrentAnimation = Animation.Attack;
			}
		}
		//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		if (CurrentAnimation == Animation.Idle) {
			Idle = true;
		} else {
			Idle = false;
		}
		if (CurrentAnimation == Animation.Walk) {
			if (Input.GetKeyDown (KeyCode.LeftControl)) {
				CurrentAnimation = Animation.Run;
			} else {
				Walk = true;
			}
		} else {
			Walk = false;
		}
		if (CurrentAnimation == Animation.Jump){
			if (Controller.isGrounded == true) {
				Jump = false;
			} else {
				Jump = true;
			}
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Jumping Down")) {
				CurrentAnimation = Animation.Idle;
			}
		}
		 else {
			Jump = false;
		}
		if (CurrentAnimation == Animation.Attack) {
			if (AttackBase.NoOfClick == 0) {
				CurrentAnimation = Animation.Idle;
				Attack = false;
			} else {
				Attack = true;
			}
		}
		if (CurrentAnimation == Animation.Run) {
			Run = true;
		}
		if (CurrentAnimation == Animation.Kick) {
			Kick = true;
		}
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		if (Idle == true && !anim.GetCurrentAnimatorStateInfo (0).IsName ("Jumping Down")){
			anim.SetBool ("Idle",true);
			Run = false;
		}
		if (Walk == true){
			anim.SetBool("Walk",true);
			anim.SetBool ("Idle",false);
		}
		if (Walk == false) {
			anim.SetBool("Walk",false);
		}
		if (Run == false) {
			anim.SetBool("Run",false);
		}
		if (Jump == true && !anim.GetCurrentAnimatorStateInfo(0).IsName("Falling") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Jumping Up") && Movement.InBattle == false){
			anim.Play ("Jumping Up");
		}
		if (Run == true) {
			anim.SetBool ("Run", true);
			GetComponent<MovementController> ().movementSpeed = 7;
			if (Input.GetKeyUp (KeyCode.LeftControl)) {
				if (Walk == true) {
					CurrentAnimation = Animation.Walk;
				}
				Run = false;
			}
		} else {
			Movement.movementSpeed = 4.5f;
		}
		if (Kick == true) {
			anim.SetBool ("Kick",true);
		}
		if (anim.GetBool ("Attack1") == true) {
			Movement.CanMove = false;
		} else {
			Movement.CanMove = true;
		}
		if (GetComponent<Status> ().CanBlock == true) {
			if (Input.GetKey (KeyCode.LeftShift)) {
				Block = true;
			} else {
				Block = false;
			}
		}
	}
	}
	public void MeleeAttack(string x){
		if (AttackBase.NoOfClick == 1 && anim.GetBool("Attack1") == false) {
			anim.SetBool (x, true);
		}
	}
	public void StartKick(){
		Movement.CanMove = false;
	}
	public void EndKick(){
		anim.SetBool ("Kick",false);
		Kick = false;
		Movement.CanMove = true ;
	}
	public void EndRevive(){
		anim.SetBool ("Revive",false);
	}
	public void CanMove(float x){
		if (x == 1) {
			Movement.CanMove = true;
		} else if (x==0){
			Movement.CanMove = false;
		}
	}
}
