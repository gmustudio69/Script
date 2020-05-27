using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleSwap : MonoBehaviour
{
	public enum States
	{
		Idle, MoveToPlayer,Attack,Wait,TurnRight,TurnLeft,Attacked
	}
	public States CurrentStates = States.Idle;
	public float Distance;
	public GameObject Player;
	public int ammo = 27;
	public float ReloadTime = 4f;
	public bool isAttack = false;
	public Status PlayerStatus;
	public EnemySet OwnStatus;
	public GameObject RayStart;
	public float GunDelay = 0.25f;
	public float LastTimeFire;
	public bool CanShot = true;
	public float LastReloadTime;
	public Animator anim;
	public GameObject VfxStart;
	public GameObject FireVfx;
	public bool reload;
	public GameObject RotationPart;
	public GameObject LookPart;
	public GameObject Bullets;
	public bool IsShoting;
	public bool Hit = false;
	public bool Dead = false;


    // Start is called before the first frame update
    void Start()
    {
		Player = GameObject.Find ("Player");
		PlayerStatus = Player.GetComponent<Status> ();
		OwnStatus = GetComponent<EnemySet> ();
		anim = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
		Hit = OwnStatus.Hit;
		Distance = Vector3.Distance (this.transform.position, Player.transform.position);
		if (Distance >= 40) {
			CurrentStates = States.Wait;
		}
		if (Distance < 50 && Distance > 20) {
			CurrentStates = States.MoveToPlayer;
		}
		if (Distance <= 20) {
			CurrentStates = States.Attack;
		}
		StatesCondition ();
    }
	void StatesCondition(){
		Vector3 LookPos = new Vector3 (Player.transform.position.x, transform.position.y , Player.transform.position.z);
		//Vector3 direction = new Vector3 (transform.rotation.x - Player.transform.rotation.x, transform.rotation.y, transform.rotation.z - Player.transform.rotation.z);
		//Quaternion rotation = Quaternion.LookRotation (direction);
		Vector3 TransformRay = new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z);
		if (CurrentStates == States.Wait) {
			anim.Play ("Idle");
		} else if (CurrentStates == States.MoveToPlayer) {
			anim.Play ("Rifle Run");
			transform.LookAt (LookPos);
			transform.position += transform.forward * OwnStatus.Speed * Time.deltaTime;
		} else if (CurrentStates == States.Attack) {
			//transform.LookAt(Vector3.Lerp(currentDirection,LookPos,OwnStatus.RotationSpeed));
			//transform.rotation = Quaternion.Lerp (transform.rotation, rotation, OwnStatus.RotationSpeed);
			transform.LookAt (LookPos);
			RaycastHit hitPlayer;
			if (Physics.Raycast (TransformRay, transform.forward, out hitPlayer)) {
				if (hitPlayer.transform.tag == "Player" && CanShot == true && reload == false) {
					LastTimeFire = Time.time;
					CanShot = false;
					IsShoting = true;
					if (ammo > 0) {
						ammo -= 1;
					}
					if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("Firing Rifle") && Hit == false) {
						anim.SetBool ("Fire",true);
					}
					Debug.DrawLine (TransformRay,hitPlayer.transform.position);
				}
			}
			if (Hit == true) {
				anim.SetBool ("Hit", true);
				if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Hit Reaction")) {
					OwnStatus.Hit = false;
				}
			} else {
				anim.SetBool ("Hit", false);
			}
			if (ammo > 0) {
				anim.SetBool ("Reload",false);
				if (Time.time - LastTimeFire >= GunDelay) {
					CanShot = true;
				}
			} else {
				anim.SetBool ("Reload",true);
				CanShot = false;
				LastReloadTime = Time.time;
				reload = true;
				if (Time.time - LastTimeFire > ReloadTime) {
					ammo = 27;
					CanShot = true;
					reload = false;
				}
			}
		}
		if (reload == true) {
			Destroy (GameObject.Find (FireVfx.name + "(Clone)"));
		}
	}
	public void FireStart(){
		if (reload == false) {
			if (IsShoting == true) {
				if (FireVfx != null) {
					GameObject FireEffect = Instantiate (FireVfx, VfxStart.transform.position, Quaternion.identity);
					FireEffect.transform.rotation = VfxStart.transform.rotation;
				}
				if (Bullets != null){
					GameObject bullet = Instantiate (Bullets, VfxStart.transform.position, Quaternion.identity) as GameObject;
					bullet.GetComponent<Bullets> ().Owner = this.gameObject;
				}

				IsShoting = false;
			}

		}
	}
	public void FireEnd(){
		if (FireVfx.gameObject != null) {
			Destroy (GameObject.Find (FireVfx.name + "(Clone)"));
		}
	}
		
}
