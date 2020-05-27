using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPDisplay : MonoBehaviour
{
	public RectTransform VisualHp;
	public RectTransform BloodLoss;
	private Status PlayerStatus;
	private float CacheY;
	private float MinXValue;
	private float MaxXvalue;
	private float CurrentHPbeforeLoss;
    // Start is called before the first frame update
    void Start()
    {
		PlayerStatus = GameObject.Find ("Player").GetComponent<Status> ();
		CacheY = VisualHp.transform.position.y;
		MaxXvalue = VisualHp.transform.position.x;
		MinXValue = VisualHp.transform.position.x - VisualHp.rect.width;
		CurrentHPbeforeLoss = PlayerStatus.hp;
    }

    // Update is called once per frame
    void Update()
    {
		HandleHealth ();
    }
	private void HandleHealth(){
		if (CurrentHPbeforeLoss > PlayerStatus.hp) {
			if (Time.time - PlayerStatus.LastTimeGetHit > 2) {
				CurrentHPbeforeLoss-= 1.5f;
			}
		}
		float currentXValue = MapValue (PlayerStatus.hp,0,PlayerStatus.maxHp,MinXValue,MaxXvalue);
		float currentLossXValue = MapValue (CurrentHPbeforeLoss,0,PlayerStatus.maxHp,MinXValue,MaxXvalue);

		VisualHp.transform.position = new Vector3 (currentXValue, CacheY);
		BloodLoss.transform.position = new Vector3 (currentLossXValue,CacheY);
		CurrentHPbeforeLoss = Mathf.Clamp (CurrentHPbeforeLoss, PlayerStatus.hp, PlayerStatus.maxHp);
	}
	private float MapValue(float x, float inMin,float inMax,float outMin,float outMax){
		return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
	}
}
