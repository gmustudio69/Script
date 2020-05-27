using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
	public SelectCamera CameraSelect;
	// Start is called before the first frame update
    void Start()
    {
		CameraSelect = GameObject.Find ("CameraSelect").GetComponent<SelectCamera>();
    }

    // Update is called once per frame
    void Update()
    {
		if (CameraSelect.CurrentCamera != this.gameObject) {
			this.gameObject.GetComponent<Cinemachine.CinemachineFreeLook> ().Priority = 10;
		}
    }
}
