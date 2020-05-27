using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySet : MonoBehaviour
{
	public float MaxHp;
	public float Hp;
	public float Speed;
	public float BasicDamage;
	public float RotationSpeed;
	public bool Hit;
	public float Resistance;
	public bool IsAttacked = false;
	public float SpeedAttack;
	public NavMeshAgent nav;
	Animator anim;
	public bool Stun = false;
	public bool CanBeStunAfterHitBlock = false;
	public float MaxAngle;
	public float MaxRadius;
	public GameObject Player;
	public bool isInFov = false;
	public float heightMultiplayer = 0.5f;
	public bool CanSteathAttack;
	[SerializeField] private float LastTimePlayerMove;
	[HideInInspector]public AimObject ClosetEnemy;
	private Status PlayerStatus;
	public bool CanMove = true;
	TimeSet Time;
    // Start is called before the first frame update
    void Start()
    {
		Hp = MaxHp;
		anim = GetComponent<Animator> ();
		nav = GetComponent<NavMeshAgent> ();
		Player = GameObject.Find ("Player").gameObject;
		ClosetEnemy = GameObject.Find("ClosetEnemy").GetComponent<AimObject>();
		PlayerStatus = Player.GetComponent<Status> ();
		Time = GameObject.Find ("Time Manager").GetComponent<TimeSet> ();
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.TimeIsStop == true) {
			anim.SetBool ("Time Stop", false);
			Hit = false;
		} else {
			anim.SetBool ("Time Stop", false);
		}
		TimeSet TimeController = GameObject.Find("Time Manager").GetComponent<TimeSet>();
		if (TimeController.TimeIsStop == true) {
			CanMove = false;
		} else 
		{
			CanMove = true;
		}
		float Distance = Vector3.Distance (Player.transform.position,this.transform.position);
		Hp = Mathf.Clamp (Hp,0,MaxHp);
		if (Hp == 0) {
			anim.Play ("Die");
			GetComponent<CapsuleCollider> ().enabled = false;
			nav.speed = 0;
		}
		GetComponent<Animator> ().SetBool ("BeHit",IsAttacked);
		if (anim.speed == SpeedAttack) {
			if (!anim.GetCurrentAnimatorStateInfo (0).IsName ("attack") && !anim.GetCurrentAnimatorStateInfo (0).IsName ("attack_double")) {
				anim.speed = 1;
				Hit = false;
			}
		}
		inFov (transform,Player.transform,MaxAngle,MaxRadius);
		if (Distance <= 20) {
			if (PlayerStatus.DetectSound>= 100 && isInFov == false) {
				isInFov = true;
			}
			if (isInFov == true) {
				PlayerStatus.DetectSound = 100;
			}
		}
    }
	public void AttackOn(float DamageOnThis){
		Hp -= DamageOnThis;
		return;
	}
	void OnTriggerStay(Collider Other){
		if (Other.gameObject.CompareTag("Player HitBox")) {
			if (Time.TimeIsStop == false) {
				if (Other.GetComponent<ClickAttackSet> ().IsAttack == true && IsAttacked == false && isInFov == true) {
					Hp -= Other.GetComponent<ClickAttackSet> ().Damage;
					Debug.Log ("Hit");
					IsAttacked = true;
				}
				if (Other.GetComponent<ClickAttackSet> ().IsAttack == true && IsAttacked == false && isInFov == false) {
					Hp -= Other.GetComponent<ClickAttackSet> ().Damage;
					IsAttacked = true;
					Debug.Log ("Hit");
					isInFov = true;
				}
			} else {
				if (Other.GetComponent<ClickAttackSet> ().IsAttack == true) {
					Hp -= Other.GetComponent<ClickAttackSet> ().Damage;
					Debug.Log ("Hit");
					IsAttacked = true;
				}
			}
		}
	}
	public void EndBeenHit(){
		IsAttacked = false;
	}
	public void Heal(float x){
		Hp += x;
	}
	public void Disover(){
		//Destroy (this.gameObject);
		Destroy(this.gameObject);
		if (PlayerStatus.EnemyNear.Contains (this)) {
			PlayerStatus.EnemyNear.Remove (this);
			GameObject.Find ("ClosetEnemy").GetComponent<AimObject> ().LockOff ();
		}
	}
	public void EndStun(){
		Stun = false;
	}
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (transform.position,MaxRadius);

		Vector3 fovline1 = Quaternion.AngleAxis (MaxAngle,transform.up)*transform.forward * MaxRadius;
		Vector3 fovline2 = Quaternion.AngleAxis (-MaxAngle, transform.up) * transform.forward * MaxRadius;

		Gizmos.color = Color.blue;
		Gizmos.DrawRay (transform.position,fovline1);
		Gizmos.DrawRay (transform.position,fovline2);
		if (isInFov == true) 
		{
			Gizmos.color = Color.red;
		} else 
		{
			Gizmos.color = Color.green;
		}
		Gizmos.DrawRay (transform.position,(Player.transform.position-transform.position).normalized *MaxRadius);

		Gizmos.color = Color.black;
		Gizmos.DrawRay (transform.position,transform.forward * MaxRadius);
	}
	public void inFov (Transform checkingObject, Transform target, float maxAngle, float maxRadius)
	{
		Vector3 directionBetween = (target.transform.position - checkingObject.transform.position).normalized;
		directionBetween.y *= 0;
		RaycastHit hit;
		if (Physics.Raycast (checkingObject.position + Vector3.up * heightMultiplayer, (target.position - checkingObject.position).normalized, out hit, maxRadius)) 
		{
			if (hit.transform.gameObject.tag == "Player") 
			{
				float angle = Vector3.Angle (checkingObject.forward+ Vector3.up * heightMultiplayer, directionBetween);
				if (angle <= maxAngle) 
				{
					isInFov = true;
				}
			}
		}
					
	}
}
