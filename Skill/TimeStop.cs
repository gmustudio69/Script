using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
	public float lv = 1;
	public SkillSet MainScript;
	public bool Use;
	private float SecondCanStop;
    public GameObject TSEffect;
    private GameObject Player;
	private float CurrentCooldown;
	[HideInInspector]public enum Type{Normal,Special,Attack};
	public Type CurrentType;
    // Start is called before the first frame update
    void Start()
    {
		MainScript = GetComponent<SkillSet> ();
        Player = GameObject.Find("Player");
		CurrentCooldown = MainScript.TimeCooldown;
    }

    // Update is called once per frame
    void Update()
	{
		Use = MainScript.Use;
		SecondCanStop = (lv * 1)+1;
		if (Use == true) 
		{
			StopTime (SecondCanStop);
		}
		if (CurrentType == Type.Special) {
			if (CurrentCooldown == MainScript.TimeCooldown) MainScript.TimeCooldown -= 5;
			if (SecondCanStop != 2)SecondCanStop = 2;
		} else{
			if (CurrentCooldown != MainScript.TimeCooldown) MainScript.TimeCooldown += 5;
		}
    }
	public void StopTime(float Second){
		if (CurrentType != Type.Attack) {
			GameObject.Find ("Time Manager").GetComponent<TimeSet> ().TimeStoped (Second);
			Instantiate(TSEffect,new Vector3 (Player.transform.position.x,Player.transform.position.y+1,Player.transform.position.z),Player.transform.rotation);
		}
		MainScript.Use = false;
        MainScript.CanUse = false;
	}
}
