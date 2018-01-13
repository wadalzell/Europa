using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pistol : MonoBehaviour {

	public GameObject arms;
	public GameObject bulletSpawn;
	public GameObject weaponHolder;
	Animator armsAnimator;
	Animator cameraAnimator;
	Animator skyboxCameraAnimator;
	Animator weaponAnimator;

	GameObject player;
	GameObject cameraHolder;
	GameObject skyboxCameraHolder;
	private playerMove playerMove;
	private aimDownSights aimdownsights;
	private healthController healthcontroller;

	float playerSpeed;
	bool sprinting;
	public bool reloading;
	bool canADS;
	bool grounded;
	bool firing;

	public float fireSpeedModifier;

	public int magSize;
	int rounds;
	public float fireCounter;
	public float fireRate;
	float reloadCounter;
	public float reloadRate;

	public Quaternion bulletForward;
	public Vector3 aimPos;

	AudioSource weaponAudio;
	public AudioClip weaponFire;
	public AudioClip weaponReload;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		skyboxCameraHolder = GameObject.FindGameObjectWithTag ("skyboxViewer");
		cameraHolder = GameObject.FindGameObjectWithTag ("cameraHolder");
		playerMove = player.GetComponent<playerMove>();
		aimdownsights = weaponHolder.GetComponent<aimDownSights> ();
		healthcontroller = player.GetComponent<healthController> ();
		armsAnimator = arms.GetComponent<Animator>();
		cameraAnimator = cameraHolder.GetComponent<Animator> ();
		skyboxCameraAnimator = skyboxCameraHolder.GetComponent<Animator> ();
		weaponAnimator = GetComponent<Animator> ();
		rounds = magSize;

		weaponAudio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		playerSpeed = playerMove.velocityMag;
		sprinting = playerMove.sprinting;
		grounded = playerMove.grounded;

		fireCounter += Time.deltaTime;
		Vector3 bulletDirection = Camera.main.transform.forward;
		bulletForward = Camera.main.transform.rotation;

		if (Input.GetButton ("Reload")&&!sprinting && !reloading) {
			reloadMechanics ();
		}

		if(reloadCounter>0){
			reloadCounter+=Time.deltaTime;
		}
			
		if(reloading && reloadCounter>reloadRate){
			reload();
			aimdownsights.canADS = true;
			reloading = false;
		}

		if (armsAnimator.GetCurrentAnimatorStateInfo (0).IsName ("fire")) {
			firing = true;
		} else {
			firing = false;
		}

		if (Input.GetButton("Fire1") && !firing) {
			if (rounds == 0 && !reloading) {
				reloadMechanics ();
			}else if (fireCounter > fireRate && !sprinting && rounds > 0 && !reloading) {
				shoot ();
			}
		}

		healthcontroller.weaponAmmo_01_max = magSize;
		healthcontroller.weaponAmmo_01 = rounds;

		armsAnimator.SetFloat("playerSpeed", playerSpeed);
		armsAnimator.SetBool ("sprinting", sprinting);
		armsAnimator.SetBool("grounded", grounded);
		armsAnimator.SetBool ("firing", firing);
		armsAnimator.SetBool ("reloading", reloading);
		weaponAnimator.SetBool ("firing", firing);
		weaponAnimator.SetBool ("reloading", reloading);

	}

	void reloadMechanics(){
		armsAnimator.SetTrigger("reload");
		weaponAnimator.SetTrigger ("reload");
		reloading = true;
		aimdownsights.canADS = false;
		reloadCounter += Time.deltaTime;
		weaponAudio.clip = weaponReload;
		weaponAudio.Play ();
	}

	void reload(){
		rounds = magSize;
		reloading = false;
		reloadCounter = 0;
	}

	void shoot(){
		rounds--;

		weaponAudio.clip = weaponFire;
		weaponAudio.Play ();

		//Animate the shot
		armsAnimator.SetTrigger("fire");
		weaponAnimator.SetTrigger("fire");
		//cameraAnimator.SetTrigger ("shoot");
		//skyboxCameraAnimator.SetTrigger ("shoot");

		cameraHolder.GetComponent<cameraShake> ().shakeDuration = fireRate;

		//muzzleFlashRot = Random.Range (0, 360);
		//muzzleFlash.transform.rotation = Quaternion.AngleAxis (muzzleFlashRot, Vector3.forward);
		/*
		muzzleFlash.SetActive (true);
		muzzleCounter += Time.deltaTime;
		recoilCounter += Time.deltaTime;
		*/	

		//Reset the counter
		fireCounter = 0;
		//Set the bullet direction in the direction the camera is facing and add a bit of inaccuracy in there (poorly named variable. Whatever.)
		Vector3 bulletDirection = Camera.main.transform.forward;
		bulletForward = Camera.main.transform.rotation;
		//bulletDirection.x += Random.Range (-accuracy, accuracy);
		//bulletDirection.y += Random.Range (-accuracy, accuracy);

		//Use a raycast for this weapon.
		RaycastHit hit;
		//Define the raycast from the bulletspawn, in  the direction the camera is facing.
		Ray ray = new Ray (bulletSpawn.transform.position, bulletDirection);
		Debug.DrawRay (bulletSpawn.transform.position, bulletDirection*100f); //Used for debugging
		//If the raycast hit something within 1000 units...
		if (Physics.Raycast (ray, out hit, 1000f)) {
			bulletTrail ();
			//And it hit something called an enemy...
			if (hit.collider.tag == "enemy") {
				hit.collider.gameObject.GetComponent<enemy01> ().health = hit.collider.gameObject.GetComponent<enemy01> ().health - 100;
			}
			/*if(hit.transform.tag != "bulletHole"){
				GameObject bulletHolePrefab = objectPooler.SharedInstance.GetPooledObject ("bulletHole");
				if (bulletHolePrefab != null) {
					bulletHolePrefab.transform.position = hit.point;
					bulletHolePrefab.transform.rotation = Quaternion.FromToRotation (Vector3.up, hit.normal);
					bulletHolePrefab.SetActive (true);
				}
			}
			*/

		}

	}

	void bulletTrail(){
		//Instantiate (bulletTrailPrefab, bulletSpawn.transform.position, Camera.main.transform.rotation);
		GameObject bulletTrailPrefab = objectPooler.SharedInstance.GetPooledObject("playerBullet");
		if (bulletTrailPrefab != null) {
			bulletTrailPrefab.transform.position = bulletSpawn.transform.position;
			bulletTrailPrefab.transform.rotation = Camera.main.transform.rotation;
			bulletTrailPrefab.SetActive (true);
		}
	}
}
