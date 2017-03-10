using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthController : MonoBehaviour {

	public float health;
	public float healthMax;
	public float shield;
	public float shieldMax;
	public int weaponAmmo_01;
	public int weaponAmmo_01_max;
	public int weaponAmmo_02;
	public int weaponAmmo_02_max;
	public int round;
	public int wave;
	public int set;

	public GameObject shieldImage;
	public GameObject healthImage;
	public GameObject weaponAmmo_01Image;
	public GameObject weaponAmmo_02Image;
	public Text weaponAmmo_01Text;
	public Text roundText;
	public Text waveText;
	public Text setText;

	private barController healthCon;
	private barController shieldCon;
	private barController weaponAmmo_01Con;
	private barController weaponAmmo_02Con;

	// Use this for initialization
	void Start () {
		healthMax = 100;
		health = healthMax;
		shieldMax = 100;
		shield = shieldMax;

		healthCon = healthImage.GetComponent<barController> ();
		shieldCon = shieldImage.GetComponent<barController> ();
		weaponAmmo_01Con = weaponAmmo_01Image.GetComponent<barController> ();
	}
	
	// Update is called once per frame
	void Update () {
		healthCon.contentMax = healthMax;
		healthCon.contentVal = health;
		shieldCon.contentMax = shieldMax;
		shieldCon.contentVal = shield;
		weaponAmmo_01Con.contentMax = weaponAmmo_01_max;
		weaponAmmo_01Con.contentVal = weaponAmmo_01;
		weaponAmmo_01Text.text = weaponAmmo_01Con.contentVal.ToString("#");
		roundText.text = "Round: " + round.ToString("#");
		waveText.text = "Wave: " + wave.ToString ("#");
		setText.text = "Set: " + set.ToString ("#");
	}
}
