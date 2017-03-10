using UnityEngine;
using System.Collections;

public class headBob : MonoBehaviour {

	float timer = 0f;
	float bobSpeed = 0.3f;
	float bobAmt= 0.1f;
	float mid = 0f;

	// Update is called once per frame
	void FixedUpdate () {
		//Also found this one online.
		if (GameObject.FindGameObjectWithTag("Player").GetComponent<playerMove> ().grounded) {

			float waveSlice = 0.0f;
			float sideSpeed = Input.GetAxis ("Horizontal");
			float forwardSpeed = Input.GetAxis ("Vertical");

			Vector3 myPosition = transform.localPosition;

			if (Mathf.Abs (sideSpeed) == 0 && Mathf.Abs (forwardSpeed) == 0) {
				timer = 0.0f;
			} else {
				waveSlice = Mathf.Sin (timer);
				timer = timer + bobSpeed;
				if (timer > Mathf.PI * 2) {
					timer = timer - (Mathf.PI * 2);
				}
			}
			if (waveSlice != 0) {
				float translateChange = waveSlice * bobAmt;
				float totalAxes = Mathf.Abs (sideSpeed) + Mathf.Abs (forwardSpeed);
				totalAxes = Mathf.Clamp (totalAxes, 0.0f, 1.0f);
				translateChange = totalAxes * translateChange;
				myPosition.y = mid + translateChange;
			} else {
				myPosition.y = mid;
			}
			transform.localPosition = myPosition;
		}
	}
}
