using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudRotate : MonoBehaviour {

	public float rotSpeed = 1;

	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, rotSpeed);
	}
}
