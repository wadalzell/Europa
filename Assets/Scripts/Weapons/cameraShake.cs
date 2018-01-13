using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour {

	Vector3 defaultPosition;

	float shakeDecay;
	float shakeIntensity = 0.2f;

	public float shakeDuration = 0f;
	public float decreaseFactor = 1f;

	void Start(){
		defaultPosition = transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
		if (shakeDuration > 0) {
			transform.localPosition = defaultPosition + Random.insideUnitSphere * shakeIntensity;
			shakeDuration -= Time.deltaTime * decreaseFactor;
		} else {
			shakeDuration = 0f;
			transform.localPosition = defaultPosition;
		}
	}
}
