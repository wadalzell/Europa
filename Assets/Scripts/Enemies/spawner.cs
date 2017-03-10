using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour {

	public float upperTimeLimit=10f;
	public float lowerTimeLimit=5f;
	public float time;
	public float counter;

	public Vector3 spPos;
	public Vector3 spRot;

	public GameObject waveController;

	// Use this for initialization
	void Start () {
		spPos = transform.position;
		spRot = transform.up;
		timeSet ();
		waveController = GameObject.FindGameObjectWithTag ("waveController");
	}
	
	// Update is called once per frame
	void Update () {
		counter += Time.deltaTime;
		if (counter > time) {
			spawn();
		}
	}

	void timeSet(){
		time = Random.Range (lowerTimeLimit, upperTimeLimit);
	}

	void spawn(){
		counter = 0;
		GameObject enemy = objectPooler.SharedInstance.GetPooledObject ("enemy");
		if (enemy != null) {
			enemy.transform.position = spPos;
			enemy.transform.rotation = Quaternion.identity;;
			enemy.SetActive (true);
			waveController.GetComponent<waveController> ().enemyToSpawn--;
		}
	}
}
