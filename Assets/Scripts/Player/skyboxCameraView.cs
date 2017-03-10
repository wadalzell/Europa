using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyboxCameraView : MonoBehaviour {

	public float verticalRotation = 0;
	float rotLR = 0;
	public float rotModifier=6;
	public float rotLimit = 90.0f;

	GameObject skyboxCamera;
	// Use this for initialization
	void Start () {
		skyboxCamera = GameObject.FindGameObjectWithTag ("skyboxCamera");
	}

	void Update(){
		rotLR = Input.GetAxis ("Mouse X")*rotModifier;
		transform.Rotate (0, rotLR, 0);
		verticalRotation -=Input.GetAxis ("Mouse Y")*rotModifier;
		verticalRotation = Mathf.Clamp (verticalRotation, -rotLimit, rotLimit);
		skyboxCamera.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);
	}
}
