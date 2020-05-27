using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour {
	public float hpCount = 1;
	public float maxHp;
	public float hp;
	public Slider healthbar;
	public float CurrentDamage;
	public float BaseDamage;
	public float DamageBonus = 1;
	public bool IsBasicAttack = false;
	public bool isAttacked = false;
	public bool Block = false;
	public float BlockValue = 0;
	public float MaxDamageCanBlock = 100;
	public float LastTimeGetHit;
	public bool CanBlock = true;
	[SerializeField]private float LastBreakTime;
	public bool CanStealth;
	public int MaxBasicAttack;
	public int HowManyEnemy;
	public int EnemyCount = 0;
	[HideInInspector]public bool IsProjective = false;
	[HideInInspector]public bool ShowWeapon = false;
	public enum AllWeapon{
		Arm,Sword,Spear,Gun,Axe,Bow,Katana
	}
	public AllWeapon CurrentWeapon;
	public bool LockCloset = true;
	[HideInInspector]public float LastTimeShow;
	public enum AttackType
	{
		isTrue,isFalse
	}
	public GameObject MainAttack;
	public bool Invincible = false;
	private MovementController Movement;
	public List<EnemySet> EnemyNear;
	public float DetectSound;
	private float LastTimeMove;
	private Animator anim;
	[HideInInspector]public AimObject Closet;
	// Use this for initialization
	private void Start () {
		maxHp = 100 * hpCount;
		hp = maxHp;
		Movement = GetComponent<MovementController> ();
		anim = GetComponent<Animator> ();
		Closet = GameObject.Find ("ClosetEnemy").GetComponent<AimObject> ();

	}
	
	// Update is called once per frame
	private void Update () {
		EnemyCount = Mathf.Clamp (EnemyCount,0,EnemyNear.Count);
		if (ShowWeapon == true) {
			if (Time.time - LastTimeShow > 2) {
				ShowWeapon = false;
			}
		}
		if (Block == true) {
			if (ShowWeapon == false) {
				ShowWeapon = true;
			}
			LastTimeShow = Time.time;
		}
		if (EnemyNear.Count > 0) {
			if (Input.GetKeyDown (KeyCode.X)) {
				if (GameObject.Find("CameraSelect").GetComponent<SelectCamera> ().CurrentCamera.gameObject != GameObject.Find ("LockTarget").gameObject && GetComponent<MovementController> ().lockTarget == false) {
					GameObject.Find("CameraSelect").GetComponent<SelectCamera> ().CurrentCamera = GameObject.Find ("LockTarget").gameObject;
					GetComponent<MovementController> ().lockTarget = true;
				} else {
					GameObject.Find("CameraSelect").GetComponent<SelectCamera> ().CurrentCamera = GameObject.Find ("Player Camera").gameObject;
					GetComponent<MovementController> ().lockTarget = false;
					Closet.PlayerStatus.LockCloset = true;
				}
			}
			if (Movement.move == true && anim.GetBool ("Run") == false) {
				DetectSound += 10 * Time.deltaTime;
				LastTimeMove = Time.time;
			}
			if (Movement.move == true && anim.GetBool ("Run") == true) {
				DetectSound += 30 * Time.deltaTime;
				LastTimeMove = Time.time;
			}
		}
		if (DetectSound > 0) {
			if (Time.time - LastTimeMove > 4 || EnemyNear.Count == 0) {
				DetectSound--;
			} 
		}
		if (Movement.lockTarget == true) {
			if (LockCloset == true) {
				if (Input.GetKeyUp (KeyCode.Tab)) {
					if (EnemyNear.Count > 0) {
						if (EnemyCount != EnemyNear.Count) {
							EnemyCount++;
						} else {
							EnemyCount = 0;
						}
						LockCloset = false;
					}
				}
			} else if (EnemyCount == EnemyNear.Count - 1) {
				if (Input.GetKeyUp (KeyCode.Tab)) {
					EnemyCount = 0;
					LockCloset = false;
				}
			} else if (EnemyNear.Count >= 2) {
				if (EnemyCount < EnemyNear.Count - 1) {
					if (Input.GetKeyUp (KeyCode.Tab)) {
						EnemyCount++;
					}
				} else if (EnemyCount == EnemyNear.Count - 1) {
					if (Input.GetKeyUp (KeyCode.Tab)) {
						EnemyCount = 0;
					}
				}
			}
		}
		DetectSound = Mathf.Clamp (DetectSound, 0, 100);
		CurrentDamage = BaseDamage + GetComponent<SkillConfig> ().ClickAttack.gameObject.GetComponent<BaseWeapon> ().BasicDamage * DamageBonus;
		maxHp = 100 * hpCount;
		if (hp > maxHp){
			hp = maxHp;
		}
		if (hp <= 0){
			hp = 0;
			dead ();
		}
		if (BlockValue > 0) {
			if (Time.time - LastTimeGetHit > 4) {
				BlockValue--;
			}
		}
		BlockValue = Mathf.Clamp (BlockValue,0,MaxDamageCanBlock);
		Block = GetComponent<Animator> ().GetBool ("Block");
		if (Block == true) {
			GetComponent<MovementController> ().CanMove = false;
		}
		if (BlockValue == MaxDamageCanBlock) {
			BlockValue = 0;
			CanBlock = false;
			GetComponent<Animator> ().SetBool ("Tired",true);
			if (!GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Tired")) {
				GetComponent<Animator> ().Play ("Tired");
			}
			LastBreakTime = Time.time;
		}

		if (CanBlock == false) {
			if (Time.time - LastBreakTime > 3) {
				CanBlock = true;
				GetComponent<Animator> ().SetBool ("Tired",false);
				GetComponent<Animator> ().SetBool ("Attack1",false);
				GetComponent<Animator> ().SetBool ("Attack2",false);
				GetComponent<Animator> ().SetBool ("Attack3",false);
				GetComponent<Animator> ().SetBool ("Walk",false);
				GetComponent<Animator> ().SetBool ("Run",false);
				GetComponent<Animator> ().SetBool ("Kick",false);
			}
		}
		if (CurrentWeapon == AllWeapon.Arm) {
			GetComponent<SkillConfig> ().SetSkill (SkillConfig.Type.click, GameObject.Find ("UnArm"));
		}
		if (CurrentWeapon == AllWeapon.Katana) {
			GetComponent<SkillConfig> ().SetSkill (SkillConfig.Type.click, GameObject.Find ("KatanaAttack"));
		}
	}
	void dead(){
		GetComponent<Animator> ().SetBool ("Die",true);
		GetComponent<MovementController> ().CanMove = false;
		GetComponent<CapsuleCollider> ().enabled = false;
	}
	public void GetDamage(float x){
		if (Invincible == false) {
			hp -= x;
			return;
		}
	}
	public void BlockDamage(float x){
		if (Invincible == false) {
			BlockValue += x;
			return;
		}
	}
	public void SetAttack(AttackType CurrentAttackType){
		if (CurrentAttackType == AttackType.isTrue) {
			IsBasicAttack = true;

		}
		if (CurrentAttackType == AttackType.isFalse) {
			IsBasicAttack = false;
		}
			
		return;
	}
	public void outOfAttack(){
		MainAttack = null;
	}
	public void OnTriggerStay(Collider Other){
		if (Other.gameObject.tag == "Enemy Hit Box") {
			EnemyHitBox OtherHitbox = Other.gameObject.GetComponent<EnemyHitBox> ();
			if (OtherHitbox.Hit == true) {
				if (IsBasicAttack == false && GameObject.Find("Time Manager").GetComponent<TimeSet>().TimeIsStop == false) {
					if (isAttacked == false && BlockValue < MaxDamageCanBlock && Block == false && GetComponent<Animator> ().GetBool ("Roll") != true) {
						GetDamage (OtherHitbox.Owner.GetComponent<EnemySet> ().BasicDamage);
						GetComponent<Animator> ().SetBool ("Hit", true);
						LastTimeGetHit = Time.time;
						isAttacked = true;
					}
					if (isAttacked == false && BlockValue < MaxDamageCanBlock && Block == true && GetComponent<Animator> ().GetBool ("Roll") != true) {
						BlockDamage (OtherHitbox.Owner.GetComponent<EnemySet> ().BasicDamage);
						GetComponent<Animator> ().SetBool ("Hit", true);
						LastTimeGetHit = Time.time;
						if (OtherHitbox.Owner.GetComponent<EnemySet> ().CanBeStunAfterHitBlock == true) {
							OtherHitbox.Owner.GetComponent<EnemySet> ().Stun = true;
						}
						isAttacked = true;
					}
				}
			}
		}
	}
	public void StartClickAttack(){
		IsBasicAttack = true;
	}
	public void EndClickAttack(){
		IsBasicAttack = false;
	}
	public void EndStrikeCancel(){
		GetComponent<Animator> ().SetBool ("StrikeCancel",false);
	}
	public void SetSpeed(float speed){
		GetComponent<Animator> ().speed = speed;
	}
	public void EndHit(){
		GetComponent<Animator>().SetBool("Hit",false);
		GetComponent<MovementController>().CanMove = true;
		isAttacked = false;
	}
}
