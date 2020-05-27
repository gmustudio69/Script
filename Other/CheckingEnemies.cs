using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckingEnemies : MonoBehaviour
{
	private GameObject Player;
	private Status PlayerStatus;
    // Start is called before the first frame update
    void Start()
    {
		Player = GameObject.Find ("Player");
		PlayerStatus = Player.GetComponent<Status> ();
    }

    // Update is called once per frame
    void Update()
    {
    }
	public void OnCollisionEnter(Collision Other){
		if (Other.gameObject.CompareTag("Enemy")) {
			PlayerStatus.EnemyNear.Add(Other.gameObject.GetComponent<EnemySet> ());
		}
	}
	public void OnCollisionExit(Collision Other){
		if (Other.gameObject.CompareTag("Enemy")) {
			PlayerStatus.EnemyNear.Remove(Other.gameObject.GetComponent<EnemySet> ());
		}
	}
}
