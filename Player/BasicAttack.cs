using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
	public float LastTimeClicked;
	public float ComboDelay = 0.9f;
	public int NoOfClick = 0;
	public Animator anim;
	public int MaxAttack;
	public Status PlayerStatus;
    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator> ();
		PlayerStatus = GetComponent<Status> ();
    }

    // Update is called once per frame
    void Update()
    {
		MaxAttack = PlayerStatus.MaxBasicAttack;
		if (GetComponent<MovementController> ().InBattle == true) {
			if (Input.GetMouseButtonDown (0)) {
				if (PlayerStatus.ShowWeapon == false) {
					PlayerStatus.ShowWeapon = true;
					PlayerStatus.LastTimeShow = Time.time;
				}
				NoOfClick++;
				LastTimeClicked = Time.time;
			}
		}
		if (Time.time - LastTimeClicked > ComboDelay) {
			NoOfClick = 0;
		}
		MaxAttack = NoOfClick = Mathf.Clamp (NoOfClick,0,MaxAttack);
		if (anim.GetBool ("Attack1") == false && NoOfClick == 1) {
			anim.SetBool ("Attack1",true);
		}
		if (anim.GetBool ("Attack1") == false) {
			NoOfClick = 0;
		}
		if (anim.GetBool ("Attack1") == true && anim.GetBool ("Attack3") == false && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3")) {
			anim.SetBool ("Attack1",false);
		}
    }
	public void EndOffAttack1(){
		if (NoOfClick >= 2) {
			anim.SetBool ("Attack2",true);
		} else {
			NoOfClick = 0;
			anim.SetBool ("Attack1",false);
		}
	}
	public void EndOffAttack2(){
		if (NoOfClick >= 3) {
			anim.SetBool ("Attack3",true);
		} else {
			NoOfClick = 0;
			anim.SetBool ("Attack2",false);
			anim.SetBool ("Attack1",false);
		}
	}
	public void EndOffAttack3(){
		anim.SetBool ("Attack1",false);
		anim.SetBool ("Attack2",false);
		anim.SetBool ("Attack3",false);
		NoOfClick = 0;
	}
}
