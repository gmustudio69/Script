using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
	public bool IsMoveForward = false;
	public float speed;
	public float TimeFloat;
	public float LastTimeMoveForward;
	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (IsMoveForward == true) {
			transform.position += transform.forward * speed;
			if (Time.time - LastTimeMoveForward >= TimeFloat) {
				speed = 0;
				TimeFloat = 0;
				IsMoveForward = false;
			}
		}
		if (Input.GetKeyDown(KeyCode.LeftControl)){
			ScrollShort ();
		}
    }
	public void MoveABit(float Speed){
		moveForward (Speed, 0.25f);
	}
	public void moveForward(float x, float y){
		LastTimeMoveForward = Time.time;
		speed = x;
		TimeFloat = y;
		IsMoveForward = true;
	}
	public void ScrollShort(){
		moveForward (0.25f,0.25f);
	}
}
