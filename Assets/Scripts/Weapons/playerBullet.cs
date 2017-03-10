using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBullet : MonoBehaviour {

	public float speed = 1000f;
	public GameObject bulletSpawn;
	public TrailRenderer trail;
	float counter;

	// Use this for initialization
	void Start () {
		trail = GetComponent<TrailRenderer> ();
		bulletSpawn = GameObject.FindGameObjectWithTag ("bulletSpawn");
		transform.position = bulletSpawn.transform.position;
	}

	void OnEnable(){
		counter = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
		counter +=Time.deltaTime;
		if (counter > 0.5f) {
			trail.Clear();
			this.gameObject.SetActive (false);
		}
	}

	void OnCollisionEnter(Collision col){
		trail.Clear();
		this.gameObject.SetActive (false);
	}
}
