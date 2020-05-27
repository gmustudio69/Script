using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
	public EnemySet MainScript;
	public bool Hit = false;
	public GameObject Owner;
    // Start is called before the first frame update
    void Start()
    {
		MainScript = Owner.GetComponent<EnemySet> ();
    }

    // Update is called once per frame
    void Update()
    {
		Hit = MainScript.Hit;
    }
}
