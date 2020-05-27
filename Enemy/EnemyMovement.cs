using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
	public NavMeshAgent nav;
	public GameObject Player;
	public GameObject ThisGameObject;
	public float AttackRange;
	public float Distance;
	public Animator anim;
	public bool IsTrigger;
	public float MoveRange;
	public Vector3 DefautPos;
	void Start()
	{
		Player = GameObject.Find ("Player");
	}
	void Update()
	{
	}
	public void MoveToDefautPos(){
		nav.SetDestination (DefautPos);
		nav.speed = 3;
	}
}
