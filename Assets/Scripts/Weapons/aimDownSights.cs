using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aimDownSights : MonoBehaviour {
	public float nextPos = 0.0f;
	float nextField = 40.0f;
	float dampVelocity = 0.4f;
	public float aimSpeed = 15f;
	public float accuracy;
	public bool ADS;
	public bool canADS=true;
	public bool reloading;
	bool sprinting;

	//Leave the camera public!
	public Camera skyboxCamera;

	private playerMove playerMoveScript;
	private pistol weaponScript;

	public GameObject arms;
	public GameObject player;
	Animator armsAnimator;
	Animator weaponHolderAnimator;

	public Vector3 defaultPos;
	public Vector3 aimPos;
	public Vector3 targetPos;
	public Vector3 currentPos;

	public GameObject weapon01;
	public GameObject weapon02;
	public GameObject activeWeapon;

	void Start(){
		player = GameObject.FindGameObjectWithTag ("Player");
		playerMoveScript = player.GetComponent<playerMove> ();
		canADS = true;
		weaponHolderAnimator = GetComponent<Animator> ();
		armsAnimator = arms.GetComponent<Animator> ();
		defaultPos = transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
		//reloading = GameObject.FindGameObjectWithTag("activeWeapon").GetComponent<pistol>().reloading;
		//No idea how this works. Found it online.
		float newField = Mathf.SmoothDamp (Camera.main.fieldOfView, nextField, ref dampVelocity, 0.05f);

		activeWeapon = GameObject.FindGameObjectWithTag ("activeWeapon");

		sprinting = playerMoveScript.sprinting;

		if (Input.GetButton ("Fire2")) {
			if (canADS && !sprinting && !reloading) {
				targetPos = GameObject.FindGameObjectWithTag ("activeWeapon").GetComponent<pistol> ().aimPos;
				aim ();
			} else {
				targetPos = defaultPos;
				hip ();
			}
		}else{
			targetPos = defaultPos;
			hip ();
		}
		Camera.main.fieldOfView = newField;
		skyboxCamera.fieldOfView = newField;
		transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPos, aimSpeed*Time.deltaTime);

		armsAnimator.SetBool ("ADS", ADS);

	}

	void aim(){
		nextField = 40.0f;
		nextPos = -0.175f;
		accuracy = 0.001f;
		ADS = true;
		//targetPos = GameObject.FindGameObjectWithTag ("activeWeapon").GetComponent<pistol> ().aimPos;
	}

	void hip(){
		nextField = 60.0f;
		nextPos = 0f;
		accuracy = 0.02f;
		ADS = false;
		//targetPos = defaultPos;
	}
}
