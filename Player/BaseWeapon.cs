using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
	public float BasicDamage;
	public GameObject Player;
	public GameObject WeaponObject;
	public Status.AllWeapon ThisWeaponType;
	public bool CanStealth;
	public int MaxAttackCan;
    // Start is called before the first frame update
    void Start()
    {
		Player = GameObject.Find ("Player");
    }

    // Update is called once per frame
    void Update()
    {
		if (Player.GetComponent<Status> ().CurrentWeapon == ThisWeaponType) {
			Player.GetComponent<Status> ().CanStealth = CanStealth;
			Player.GetComponent<Status> ().MaxBasicAttack = MaxAttackCan;
			if (WeaponObject != null) {
				if (Player.GetComponent<MovementController> ().InBattle == true) {
					WeaponObject.gameObject.SetActive (true);
				} else {
					WeaponObject.gameObject.SetActive (false);
					Player.GetComponent<Animator> ().speed = 1;
					Player.GetComponent<Status> ().MainAttack = null;
					Player.GetComponent<Animator> ().SetBool("Attack1",false);
					Player.GetComponent<Animator> ().SetBool("Attack2",false);
					Player.GetComponent<Animator> ().SetBool("Attack3",false);
				}
			}
		}
		if (Player.GetComponent<Status> ().CurrentWeapon != ThisWeaponType) {
			if (WeaponObject != null) {
				WeaponObject.gameObject.SetActive (false);
			}
		}
    }
}
