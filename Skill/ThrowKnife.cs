using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnife : MonoBehaviour
{
	public SkillSet Mainscript;
	private bool Use, CanUse;
	private float LastTimeUse;
	private Status PlayerStatus;
	private MovementController PlayerMovement;
    // Start is called before the first frame update
    void Start()
    {
		Mainscript = GetComponent<SkillSet> ();
		PlayerStatus = GameObject.Find ("Player").GetComponent<Status> ();
		PlayerMovement = GameObject.Find ("Player").GetComponent<MovementController> ();
    }

    // Update is called once per frame
    void Update()
    {
		Use = Mainscript.Use;
		CanUse = Mainscript.CanUse;
		if (Use == true) {
			PlayerStatus.IsProjective = true;
			PlayerMovement.AimCenter = true;

		}
    }
}
