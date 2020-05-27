using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSEffect : MonoBehaviour
{
    public int MaxScale;
    bool IsDecrease;
    private TimeSet TimeManager;
    MovementController PLayerMovement;
    private bool CanMove;
    private bool StartStopTime;
    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        PLayerMovement = Player.GetComponent<MovementController>();
        TimeManager = GameObject.Find("Time Manager").GetComponent<TimeSet>();
        StartStopTime = TimeManager.StartStopTime;
    }

    // Update is called once per frame
    void Update()
    {
        var scale = transform.localScale;
        if (scale.x < MaxScale && IsDecrease == false)
        {
            Debug.Log("False");
            PLayerMovement.ChangeMove(false);
            transform.localScale = new Vector3(scale.x + 1, scale.y + 1, scale.z + 1);
        }
        if (scale.x >= MaxScale && IsDecrease == false)
        {
            IsDecrease = true;
        }
        if (IsDecrease == true)
        {
            transform.localScale = new Vector3(scale.x - 1, scale.y - 1, scale.z - 1);
            if (scale.x <= 0)
            {
				Destroy (this.gameObject);
            }
        }
    }
}
