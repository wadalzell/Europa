using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimDownSights : MonoBehaviour {
	float nextPos = 0.0f;
	float nextField = 40.0f;
	float dampVelocity = 0.4f;
	public float accuracy;
	public bool ADS;
	public bool canADS=true;
	bool sprinting;

	//Leave the camera public!
	public Camera skyboxCamera;

	private playerMove PlayerMove;


	void Start(){
		PlayerMove = GetComponent<playerMove> ();
		canADS = true;
	}

	// Update is called once per frame
	void Update () {
		//No idea how this works. Found it online.
		float newField = Mathf.SmoothDamp (Camera.main.fieldOfView, nextField, ref dampVelocity, 0.05f);

		sprinting = PlayerMove.sprinting;

		if (Input.GetButton ("Fire2")) {
			if (canADS && !sprinting) {
				aim ();
			} else {
				hip ();
			}
		}else{
			hip ();
		}
		Camera.main.fieldOfView = newField;
		skyboxCamera.fieldOfView = newField;
	}

	void aim(){
		nextField = 40.0f;
		nextPos = -0.175f;
		accuracy = 0.001f;
		ADS = true;
	}

	void hip(){
		nextField = 60.0f;
		nextPos = 0f;
		accuracy = 0.02f;
		ADS = false;
	}
}
