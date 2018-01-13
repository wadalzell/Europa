using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasController : MonoBehaviour {

	public GameObject crosshair;
	public GameObject redDot;
	public GameObject weaponHolder;
	public float counter;

	public bool ADS;
	public bool shooting;

	private aimDownSights AimDownSights;

	// Use this for initialization
	void Start () {
		AimDownSights = weaponHolder.GetComponent<aimDownSights> ();
	}
	
	// Update is called once per frame
	void Update () {
		ADS = AimDownSights.ADS;
		if (ADS) {
			counter += Time.deltaTime;
			crosshair.SetActive (false);
		} else {
			redDot.SetActive (false);
			crosshair.SetActive (true);
		}

		if (counter >0.15f) {
			redDot.SetActive (true);
			counter = 0;
		}
	}
}
