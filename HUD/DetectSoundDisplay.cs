using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectSoundDisplay : MonoBehaviour
{

	public Sprite zero,ss25,ss50,ss100;
	private Status PlayerStatus;
	private Image ThisImage;
    // Start is called before the first frame update
    void Start()
    {
		PlayerStatus = GameObject.Find ("Player").GetComponent<Status> ();
		ThisImage = GetComponent<Image> ();
    }

    // Update is called once per frame
    void Update()
    {
		if (PlayerStatus.DetectSound < 25) {
			ThisImage.sprite = zero;
		} 
		if (PlayerStatus.DetectSound > 25 && PlayerStatus.DetectSound < 50) {
			ThisImage.sprite = ss25;
		}
		if (PlayerStatus.DetectSound > 50 && PlayerStatus.DetectSound < 100) {
			ThisImage.sprite = ss50;
		}
		if (PlayerStatus.DetectSound == 100) {
			ThisImage.sprite = ss100;
		} 
    }
}
