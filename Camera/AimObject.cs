using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimObject : MonoBehaviour
{
	public GameObject Player;
	public GameObject CameraSelect;
	public GameObject CameraAim;
	public Image LockArt;
	public Status PlayerStatus;
	public GameObject CurrentTarget;
	public List<EnemySet> allEnemies;
	public EnemySet ClosetEnemy;
	public float CurrentDistanceToEnemy;
	public EnemySet CurrentEnemy;
	public GameObject LastClosetEnemy;
	// Start is called before the first frame update
    void Start()
    {
		CurrentTarget = null;
		Player = GameObject.Find ("Player");
		CameraSelect = GameObject.Find ("CameraSelect");
		CameraAim = GameObject.Find ("LockTarget");
		PlayerStatus = GameObject.Find ("Player").GetComponent<Status>();
    }

    // Update is called once per frame
    void Update()
    {
		FindClosetEnemy ();
    }
	void FindClosetEnemy(){
		float distanceToCloseEnemy = Mathf.Infinity;
		EnemySet ClosetEnemy = null;
		allEnemies = PlayerStatus.EnemyNear;
		foreach (EnemySet CurrentEnemy in allEnemies) {
			float distanceToEnemy = Vector3.Distance(CurrentEnemy.transform.position,Player.transform.position);
			if (distanceToEnemy < distanceToCloseEnemy) {
				distanceToCloseEnemy = distanceToEnemy;
				ClosetEnemy = CurrentEnemy;
			}
			float Distance = Vector3.Distance (Player.transform.position,ClosetEnemy.transform.position);
			if (distanceToCloseEnemy < 20) {
				if (PlayerStatus.LockCloset == true) {
					if (LastClosetEnemy != null) {
						Vector3 ClosetPos = new Vector3(LastClosetEnemy.gameObject.transform.position.x, LastClosetEnemy.gameObject.transform.position.y + 1, LastClosetEnemy.gameObject.transform.position.z);
						transform.position = ClosetPos;
						transform.rotation = ClosetEnemy.transform.rotation;
						CurrentTarget = ClosetEnemy.gameObject;
					}
				} else {
					transform.position = new Vector3 (allEnemies [PlayerStatus.EnemyCount].transform.position.x, allEnemies [PlayerStatus.EnemyCount].transform.position.y + 1, allEnemies [PlayerStatus.EnemyCount].transform.position.z);
					transform.rotation = allEnemies [PlayerStatus.EnemyCount].transform.rotation;
					CurrentTarget = allEnemies [PlayerStatus.EnemyCount].gameObject;
				}
			if (CameraSelect.GetComponent<SelectCamera> ().CurrentCamera == CameraAim.gameObject && Player.GetComponent<MovementController> ().lockTarget == true) {
					if (PlayerStatus.LockCloset == true) {
						if (LastClosetEnemy == null) {
							LastClosetEnemy = ClosetEnemy.gameObject;
						} else {
							if (distanceToEnemy > 20) {
								LastClosetEnemy = null;
								CameraSelect.GetComponent<SelectCamera> ().CurrentCamera = GameObject.Find("Player Camera").gameObject;
								Player.GetComponent<MovementController> ().lockTarget = false;
							}
						}
					} else {
						LastClosetEnemy = null;
					}

			}		
			if (distanceToCloseEnemy <= 20 && Player.GetComponent<MovementController> ().InBattle == false) {
				Player.GetComponent<MovementController> ().InBattle = true;
			}
			if (CameraSelect.GetComponent<SelectCamera> ().CurrentCamera == CameraAim.gameObject && Player.GetComponent<MovementController> ().lockTarget == true && Distance >= 50||PlayerStatus.EnemyNear.Count == 0) 
			{	
				LockOff ();
				Player.GetComponent<MovementController> ().InBattle = false;
				CurrentTarget = null;
				PlayerStatus.LockCloset = true;
			}
	}
	}
	}
	private void OnGizmosSelected(){
		Gizmos.color = Color.white;
		Gizmos.DrawRay (Player.transform.position,ClosetEnemy.transform.position);
	}
	public void LockOff()
	{
		CameraSelect.GetComponent<SelectCamera> ().CurrentCamera = GameObject.Find("Player Camera").gameObject;
		Player.GetComponent<MovementController> ().lockTarget = false;
	}
}
