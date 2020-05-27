using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSet : MonoBehaviour
{
	public bool Use = false;
	public bool CanUse;
	private float LastTimeUsed;
	public float TimeCooldown = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (CanUse == false) 
		{
			if (Time.time - LastTimeUsed > TimeCooldown) 
			{
				CanUse = true;
			}
		}
	}
	public void UseThis(bool x){
        if (x == true && CanUse == true && Use == false)
        {
            Use = true;
            CanUse = false;
            LastTimeUsed = Time.time;
        }
	}
}