using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
	public float Speed;
	public GameObject Owner;
	public float Range;
	public GameObject Player;
	public float Damage;
    // Start is called before the first frame update
    void Start()
    {
		Player = GameObject.Find ("Player");
		transform.LookAt (new Vector3(Player.transform.position.x,transform.position.y,Player.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
		Damage = Owner.GetComponent<EnemySet> ().BasicDamage;
		float Distance = Vector3.Distance (transform.position,Owner.transform.position);
		if (Distance < Range) {
			transform.position += transform.forward * Speed;
		} else if (Distance >= Range) {
			Destroy (this.gameObject);
		}
    }
	public void OnCollisionEnter(Collision Other){
		if (Other.gameObject.tag == "Player") {
			print ("Hit Player");
			Other.gameObject.GetComponent<Status> ().GetDamage (Damage);
		}
	}
}
