using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NightmareCreature : MonoBehaviour
{
	public NavMeshAgent nav;
	public float Distance;
	private Animator anim;
	private Status PlayerStatus;
	private EnemySet MainScript;
	private GameObject Player;
	private bool IsTrigger;
	[HideInInspector]public enum TF{True,False};
	bool CanMove = true;
	public bool Attacking;
	public bool CanAttack = true;
	float lastTimeAttack;
    // Start is called before the first frame update
    void Start()
    {
		nav = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		PlayerStatus = GameObject.Find("Player").GetComponent<Status> ();
		MainScript = GetComponent<EnemySet> ();
		Player = GameObject.Find ("Player");
		anim.SetBool ("Break", true);
    }

    // Update is called once per frame
    void Update()
    {
		MainScript.Hit = Attacking;
		TimeSet time = GameObject.Find ("Time Manager").GetComponent<TimeSet> ();
		anim.SetBool ("Time Stop", time.TimeIsStop);
		Distance = Vector3.Distance (Player.transform.position,this.transform.position);
		if (time.TimeIsStop == true) {
			anim.speed = 0;
			nav.speed = 0;
		} else {
			anim.speed = 1;
			if (Attacking == true) {
				anim.speed = MainScript.SpeedAttack;
			}
			if (IsTrigger == false) {
				if (MainScript.isInFov == true) {
					anim.SetBool ("Idle", true);
					anim.SetBool ("Break", false);
					IsTrigger = true;
				}
			} else {
				if (Distance <= 20 && Distance > 3) {
					Walk ();
				} else if (Distance < 3) {
					Vector3 TransformRay = new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z);
					RotateWhenAttackFalse ();
					if (CanAttack == true) {
						RaycastHit hit;
						if (Physics.Raycast (TransformRay, transform.forward, out hit)) {
							if (hit.transform.tag == ("PLayer")) {
								Debug.Log ("hit");
								StartAttack ();
							}
						}
					} else {
						if (Time.time - lastTimeAttack > 4) {
							CanAttack = true;
						} 
					}
				} else if (Distance >= 20) {
					anim.SetBool ("Break", true);
					anim.SetBool ("Idle", false);
					MainScript.isInFov = false;
					IsTrigger = false;
				}
			}
		}
    }
	public void Walk (){
		anim.SetBool ("Attack",false);
		int AttackType = 0;
		float LastTimeCheck = 0;
		bool CheckType = true;
		if (CheckType == true) {
			CheckType = false;
			AttackType = Random.Range (1, 101);
			LastTimeCheck = Time.time;
		} else {
			AttackType = 0;
			if (AttackType != 0 && Time.time - LastTimeCheck > 10 && anim.GetBool("Run Attack") == false) {
				CheckType = true;
			}
		}
		if (AttackType > 0 && AttackType < 80 && anim.GetBool("Run Attack") == false) {
			anim.SetBool ("Walk",true);
			anim.SetBool ("Run Attack",false);
			nav.SetDestination (Player.transform.position);
			ChangeMoveSpeed (3);
		} else {
			RunAttack ();
		}
	}
	public void RotateWhenAttackFalse(){
		Vector3 PlayerLookAt = new Vector3 (Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
		Quaternion rotTarget = Quaternion.LookRotation (PlayerLookAt - this.transform.position);
		if (Attacking == false) {
			transform.rotation = Quaternion.Lerp (this.transform.rotation, rotTarget, MainScript.RotationSpeed * Time.deltaTime);
		}
	}
	public void RunAttack(){
		anim.SetBool ("Walk",false);
		anim.SetBool ("Run Attack",true);
		nav.SetDestination (Player.transform.position);
		ChangeMoveSpeed (10);
	}
	public void StartAttack(){
		anim.SetBool ("Attack", true);
		CanAttack = false;
	}
	public void ChangeMoveSpeed(float Speed){
		if (CanMove == true) {
			nav.speed = Speed;
		} else {
			nav.speed = 0;
		}
	}
	//chinh co dang tan cong hay k
	public void SetAttack(TF Bool){
		if (Bool == TF.True) {
			Attacking = true;
			ChangeMove(TF.False);
			nav.SetDestination (this.transform.position);
		} else {
			Attacking = false;
			ChangeMove(TF.True);
			anim.SetBool ("Attack", false);
			anim.SetBool ("Run Attack", false);
			lastTimeAttack = Time.time;
		}
	} 
	public void ChangeMove(TF Bool){
		if (Bool == TF.True) {
			CanMove = true;
		} else {
			CanMove = false;
		}
	}
}
