using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBullet : MonoBehaviour {

	public int bulletSpeed = 1;

		// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward*Time.deltaTime * bulletSpeed);
	}

	void OnCollisionEnter(Collision col){
		this.gameObject.SetActive (false);
	}
}
