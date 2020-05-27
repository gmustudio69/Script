using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterAim : MonoBehaviour
{
	public GameObject Center;
	public GameObject MainCamera;
    public Vector3 HitPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		RaycastHit hit;
		if (Physics.Raycast (MainCamera.transform.position, MainCamera.transform.forward,out hit,Mathf.Infinity,layerMask:9)) {
            HitPosition = hit.point;
        }
        Center.transform.position = HitPosition; 
    }
	void OnDrawGizmos(){
		Gizmos.color = Color.white;
		Gizmos.DrawRay (MainCamera.transform.position, MainCamera.transform.forward);
	}
}
