using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
	public bool CanCancel;
	public Status PlayerStatus;
	public GameObject Owner;
    // Start is called before the first frame update
    void Start()
    {
		PlayerStatus = GameObject.Find ("Player").GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
