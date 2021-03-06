using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonMeleeAxe : MonoBehaviour
{
	EnemySet OwnerScript;
	[HideInInspector]public float Distance;
	private GameObject Player;
	Animator anim;
	[Header("Bool")]
	public bool Hit = false;
	public bool CanAttack = true;
	public bool Block;
	public bool IsTrigger;
	[Header("Base Stats")]
	[SerializeField]private float SpeedAttack;
	[SerializeField]private float MoveRange = 20;
	[HideInInspector]public bool IsAttack = false;
	private float AttackCooldown = 2.5f;
	[SerializeField]
	private float LastTimeAttack;

	private NavMeshAgent nav;
	public enum TF{True,False}
	public TF tf;
	private int AttackRandom;
	[HideInInspector]public bool NeedRandomValue = true;


	// Start is called before the first frame update
    void Start()
    {
		OwnerScript = this.gameObject.GetComponent<EnemySet> ();
		Player = GameObject.Find ("Player");
		anim = this.gameObject.GetComponent<Animator> ();
		nav = this.gameObject.GetComponent<NavMeshAgent> ();
    }

    // Update is called once per frame
    void Update()
    {
		Hit = OwnerScript.Hit;
		Distance = Vector3.Distance (this.transform.position, Player.transform.position);
		if (OwnerScript.CanMove == true) 
		{
			if (GameObject.Find ("Time Manager").GetComponent<TimeSet> ().TimeIsStop == true) {
				if (anim.GetBool ("BeHit") == true || anim.GetBool ("Attack") == true) {
					OwnerScript.CanMove = false;
				}
			}
			if (anim.speed ==0) anim.speed = 1;
			if (IsTrigger != true) {
				if (OwnerScript.isInFov == true) {
					IsTrigger = true;
				}
			} else if (OwnerScript.Stun == false) {
				if (Distance <= MoveRange && Distance > 2) {
					Walk ();
				}
				if (Distance > MoveRange) {
					Run ();
				}
				if (Distance <= 2) {
					Attack ();
				}
			}
			anim.SetBool ("Stun", OwnerScript.Stun);
			OwnerScript.Hit = IsAttack;
			SpeedAttack = OwnerScript.SpeedAttack;
		} 
		if (OwnerScript.CanMove == false)
		{
			anim.speed = 0;
			nav.speed = 0;
		}
	}
	public void Attacking(TF x)
	{
		if (x == TF.True) 
		{
			Stop ();
			Hit = true;
			anim.speed = SpeedAttack;
		}
		else
		{
			Hit = false;
			anim.speed = 1;
		}
	}
	public void MoveToPlayer(Vector3 x,float Speed)
	{
		nav.SetDestination (x);
		nav.speed = Speed;
	}
	void Walk()
	{
		anim.SetBool ("Walk", true);
		anim.SetBool ("Run", false);
		anim.SetBool ("AttackDouble", false);
		anim.SetBool ("Attack", false);
		MoveToPlayer (Player.transform.position, OwnerScript.Speed);
	}
	void Run()
	{
		anim.SetBool ("Walk", true);
		anim.SetBool ("Run", true);
		anim.SetBool ("AttackDouble", false);
		anim.SetBool ("Attack", false);
		MoveToPlayer (Player.transform.position, OwnerScript.Speed * 2);
	}
	void Attack(){
		Vector3 PlayerLookAt = new Vector3 (Player.transform.position.x, this.transform.position.y, Player.transform.position.z);
		Quaternion rotTarget = Quaternion.LookRotation (PlayerLookAt - this.transform.position);
		if (IsAttack == false) 
		{
			transform.rotation = Quaternion.Lerp (this.transform.rotation, rotTarget, OwnerScript.RotationSpeed * Time.deltaTime);
		} 
		if (CanAttack == true)
		{
			if (AttackRandom == 0 && NeedRandomValue == true)
			{
				AttackRandom = Random.Range (1, 5);	
			}
			if (AttackRandom == 1 || AttackRandom == 2 || AttackRandom == 3)
			{
				if (CanAttack == true)
				{
					CanAttack = false;
					anim.SetBool ("Attack", true);
					anim.SetBool ("AttackDouble", false);
				}
			} 
			if (AttackRandom == 4) 
			{
				if (CanAttack == true) 
				{
					CanAttack = false;
					anim.SetBool ("AttackDouble", true);
					anim.SetBool ("Attack", false);
				}
			}
		}
		//When Attack Not Ready
		if (CanAttack == false) 
		{
			if (Time.time - LastTimeAttack > AttackCooldown) 
			{
				CanAttack = true;
				NeedRandomValue = true;
			}
			if (anim.GetBool ("AttackDouble") == true && AttackRandom != 4) 
			{
				anim.SetBool ("AttackDouble", false);
			}
		}
		if (CanAttack == true && NeedRandomValue == false)
		{
			NeedRandomValue = true;
		}
	}
	public void Stop()
	{
		nav.speed = 0;
	}
	public void EndAttack()
	{
		AttackRandom = 0;
		anim.SetBool ("Attack", false);
		anim.SetBool ("AttackDouble",false);
		OwnerScript.CanMove = true;
	}
	public void StartAttack()
	{
		LastTimeAttack = Time.time;
		anim.SetBool ("Attack", false);
		anim.SetBool ("AttackDouble",false);
		NeedRandomValue = false;
	}
}
