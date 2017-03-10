using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy01 : MonoBehaviour {

	public float health = 100f;
	public float killTime = 10f;
	public float killCounter;
	public bool killed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			this.gameObject.SetActive (false);
		}
	}
}
