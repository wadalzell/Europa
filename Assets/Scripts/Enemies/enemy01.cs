using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RAIN.Core;

public class enemy01 : MonoBehaviour {

	public float health = 100f;
	public float killTime = 10f;
	public float killCounter;
	public bool killed;

	public GameObject player;

	public AIRig ai;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		ai.AI.WorkingMemory.SetItem<GameObject> ("player", player);
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			this.gameObject.SetActive (false);
			killed = true;
		}
		ai.AI.WorkingMemory.SetItem<float> ("Health", health);
	}
}
