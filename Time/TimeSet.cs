using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSet : MonoBehaviour
{
	public bool TimeIsStop;
	public float SecondStop;
	public bool EnemyCanTimeStop;
	public float PlayerCanMoveIn;
    public UnityStandardAssets.ImageEffects.GrayLayers Grayscale;
	private float LastTimeStop;
    public bool StartStopTime;
    private void Start()
    {
        Grayscale = GameObject.Find("Camera").GetComponent<UnityStandardAssets.ImageEffects.GrayLayers>();
    }
    void Update()
	{
        if (TimeIsStop)
        {
            if (Time.time - LastTimeStop > SecondStop && LastTimeStop != 0)
            {
                TimeIsStop = false;
                Grayscale.enabled = false;
            }
        }
		if (EnemyCanTimeStop)
		{
			MovementController Movement = GameObject.Find ("Player").GetComponent<MovementController> ();
			float LastTimeMove = 0;
			if (Time.time - LastTimeStop > SecondStop) {
				TimeIsStop = false;
			} else 
			{
				if (Movement.move == true && LastTimeMove == 0) 
				{
					LastTimeMove = Time.time;
				}
			}
			if (Time.time - LastTimeMove > PlayerCanMoveIn)
			{
				Movement.CanMove = false;
			}
			if (Time.time - LastTimeStop > SecondStop) 
			{
				EnemyCanTimeStop = false;
			}
		}
	}
	public void TimeStoped(float SecondCanStop)
	{
		if (TimeIsStop == false) 
		{
			TimeIsStop = true;
			SecondStop = SecondCanStop;
			EnemyCanTimeStop = false;
            Grayscale.enabled = true;
			LastTimeStop = Time.time;
		}
	}
	public void EnemyStopTime(float SecondCanStop, float SecondThatPlayerCanMove)
	{
		if (EnemyCanTimeStop == false)
		{
			EnemyCanTimeStop = true;
			TimeIsStop = false;
			LastTimeStop = Time.time;
			SecondStop = SecondCanStop;
			PlayerCanMoveIn = SecondThatPlayerCanMove;
		}
	}
}
