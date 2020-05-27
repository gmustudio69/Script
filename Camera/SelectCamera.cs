using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCamera : MonoBehaviour
{
	public  GameObject CurrentCamera;
    // Start is called before the first frame update
    void Start()
    {
		CurrentCamera = GameObject.Find("Player Camera");


    }

    // Update is called once per frame
    void Update()
    {
		CurrentCamera.GetComponent<Cinemachine.CinemachineFreeLook> ().Priority = 15;

	}
	public void CameraSet(GameObject x){
		CurrentCamera = x;
	}
}
