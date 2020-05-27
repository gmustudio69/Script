using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillConfig : MonoBehaviour
{
	public GameObject ClickAttack;
	public GameObject Skill1,Skill2,Skill3,Skill4;
	public enum Type{click,skill1,skill2,skill3,skill4};
	public Type CurrentSet;

	public Status PlayerStatus;
    // Start is called before the first frame update
    void Start()
    {
		PlayerStatus = GetComponent<Status> ();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButtonDown(0) && ClickAttack != null){
			PlayerStatus.MainAttack = ClickAttack;
		}
		if (Input.GetKeyDown(KeyCode.Q) && Skill1.GetComponent<SkillSet>().CanUse) {
			Skill1.GetComponent<SkillSet> ().UseThis(true);
			PlayerStatus.MainAttack = Skill1;
		}
		else if (Input.GetKeyDown(KeyCode.E) && Skill2.GetComponent<SkillSet>().CanUse) {
			Skill2.GetComponent<SkillSet> ().UseThis(true);
			PlayerStatus.MainAttack = Skill2;
		}
		else if (Input.GetKeyDown(KeyCode.R) && Skill3.GetComponent<SkillSet>().CanUse) {
			Skill3.GetComponent<SkillSet> ().UseThis(true);
			PlayerStatus.MainAttack = Skill3;
		}
    }
	public void SetSkill (Type x,GameObject y){
		switch (x) {
		case Type.click:
			ClickAttack = y;
			break;
		case Type.skill1:
			Skill1 = y;
			break;
		case Type.skill2:
			Skill2 = y;
			break;
		case Type.skill3:
			Skill3 = y;
			break;
		case Type.skill4:
			Skill4 = y;
			break;

		}
	}
	public void Out(){
		Skill1.GetComponent<SkillSet> ().UseThis(false);
		Skill2.GetComponent<SkillSet> ().UseThis(false);
		Skill3.GetComponent<SkillSet> ().UseThis(false);
		Skill4.GetComponent<SkillSet> ().UseThis(false);
	}
}
