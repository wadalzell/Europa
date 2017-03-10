	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponController : MonoBehaviour {

	GameObject player;
	public GameObject weapon01;
	public GameObject weapon02;

	float counter=0.0f;
	float switchTime=0.5f;

	Animator animator01;
	Animator animator02;

	public bool switching;

	// Use this for initialization
	void Start () {
		//One weapon must be started as stowed.
		weapon02.SetActive (false);
		animator01 = weapon01.GetComponent<Animator> ();
		animator02 = weapon02.GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		weapon02.tag = "inactiveWeapon";
		switching = false;
	}

	// Update is called once per frame
	void Update () {

		//If the switch weapon key was pressed and there has been enough time since the last weapon switch...
		if (Input.GetButtonDown("switch") && !switching) {
			//Call the weapon switch function and reset the counter.
			switchAnim();
			switching = true;
			counter += Time.deltaTime;
			player.GetComponent<aimDownSights> ().canADS = false;
		}
		if (counter > 0) {
			counter += Time.deltaTime;
		}
		if (switching && counter > switchTime) {
			weaponSwitch();
			player.GetComponent<aimDownSights> ().canADS = true;
		}
	}
		
	void switchAnim(){
		if (weapon01.activeSelf) {
			animator01.SetTrigger ("stow");
		} else {
			animator02.SetTrigger ("stow");
		}
	}

	void weaponSwitch(){
		if (weapon01.activeSelf) {
			weapon01.SetActive (false);
			weapon01.tag = "activeWeapon";
			weapon02.SetActive (true);
			weapon02.tag = "inactiveWeapon";
			counter = 0f;
			switching = false;
		} else {
			weapon01.SetActive (true);
			weapon01.tag = "inactiveWeapon";
			weapon02.SetActive (false);
			weapon02.tag = "activeWeapon";
			counter = 0f;
			switching = false;
		}
	}
}
