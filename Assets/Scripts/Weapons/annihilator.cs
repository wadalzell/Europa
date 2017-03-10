using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class annihilator : MonoBehaviour {

	public GameObject bulletSpawn;

	public float fireRate = 0.5f;
	public float damage = 25f;
	private float counter = 0;
	public float accuracy = 0.02f;
	public float speed;
	public float reloadTime = 1f;
	public float muzzleTime=0.25f;
	public float muzzleCounter;
	public float muzzleFlashRot;
	public float recoilCounter;
	public float recoilTime=0.25f;
	public float verticalVelocityMag;
	public int magSize = 10;
	public int rounds;

	public GameObject weaponModel;
	public GameObject playerBullet;
	public GameObject muzzleFlash;
	public GameObject skyboxViewer;
	public Animator animator;
	GameObject cameraHolder;
	Animator cameraAnimator;
	GameObject skyboxCameraHolder;
	Animator skyboxCameraAnimator;



	public AudioSource annihilatorAudio;
	public AudioClip fireAudio;
	public AudioClip reloadAudio;

	public bool reloading;
	public bool grounded;
	public bool switching;
	public bool ADS;
	public bool dblJump;

	public Vector3 nonADSPos;
	public Vector3 ADSPos;
	public Vector3 nextPos;

	public Vector3 cameraPos;
	public Vector3 recoilPos;
	public Vector3 nextCamPos;

	bool sprinting;
	float reloadCounter;

	private playerMove PlayerMove;
	private weaponController WeaponController;
	private aimDownSights AimDownSights;
	private skyboxCameraView SkyboxCameraView;
	private healthController HealthController;

	GameObject player;

	// Use this for initialization
	void Start () {
		//Define a few things needed for the code.
		Cursor.visible=false;
		player = GameObject.FindGameObjectWithTag ("Player");
		bulletSpawn = GameObject.FindGameObjectWithTag ("bulletSpawn");
		animator = weaponModel.GetComponent<Animator> ();
		reloading = false;
		reloadCounter = 0f;
		rounds = magSize;
		annihilatorAudio = GetComponent<AudioSource> ();
		muzzleFlash.SetActive (false);
		nonADSPos = new Vector3 (-0.07f, -0.18f, -0.15f);
		ADSPos = new Vector3 (-0.153f, -0.12f, -0.23f);
		cameraPos = new Vector3 (0, 0f, 0.5f);
		recoilPos = cameraPos - new Vector3 (0, 0.25f, 0.25f);
		cameraHolder = GameObject.FindGameObjectWithTag ("cameraHolder");
		skyboxCameraHolder = GameObject.FindGameObjectWithTag ("skyboxViewer");
		cameraAnimator = cameraHolder.GetComponent<Animator> ();
		skyboxCameraAnimator = skyboxCameraHolder.GetComponent<Animator> ();

		PlayerMove = player.GetComponent<playerMove> ();
		WeaponController = player.GetComponent<weaponController> ();
		AimDownSights = player.GetComponent<aimDownSights> ();
		SkyboxCameraView=skyboxCameraHolder.GetComponent<skyboxCameraView>();
		HealthController = player.GetComponent<healthController> ();
	}

	// Update is called once per frame
	void Update () {
		//Get required paramters from other scripts
		grounded = PlayerMove.grounded;
		speed = PlayerMove.velocityMag;
		switching = WeaponController.switching;
		ADS = AimDownSights.ADS;
		sprinting = PlayerMove.sprinting;
		verticalVelocityMag = PlayerMove.verticalVelocityMag;
		dblJump = PlayerMove.dblJump;
		HealthController.weaponAmmo_01_max = magSize;
		HealthController.weaponAmmo_01 = rounds;

		//Set all the animator mumbo jumbo
		animator.SetFloat ("speed", speed);
		animator.SetBool ("ADS", ADS);
		animator.SetBool ("reloading", reloading);
		animator.SetBool ("sprinting", sprinting);

		//Update the counter.
		counter +=Time.deltaTime;

		//This moves the wapon to an ADS position. Nothing fancy to see here
		if (ADS && !sprinting && !reloading) {
			nextPos = ADSPos;
		} else {
			nextPos = nonADSPos;
		}
		transform.localPosition = Vector3.Lerp (transform.localPosition, nextPos, 0.25f);

		//This next section handles the reloading. If the player tries to shoot, but is out of bullets, and is not already reloading, start the reload anim, set reload to
		//true and begin the reload timer.
		if (Input.GetButtonDown ("Fire1") && rounds == 0 && !reloading) {
			reloadMechanics ();
		}
		//This works the same as the if statement above, but sees if the player pressed the reload button and is not reloading.
		if(Input.GetKeyDown(KeyCode.R) && !reloading){
			reloadMechanics ();
		}
		//If the reload timer has been started, continue it. This is for animating purposes.
		if(reloadCounter>0){
			reloadCounter+=Time.deltaTime;
		}
		//If the weapon is reloading, and the reload timer is greater than the reload time (checking if the animation is complete) then actually reload the bullets.
		if(reloading && reloadCounter>reloadTime){
			reload();
			AimDownSights.canADS = true;
		}
		if (Input.GetButtonDown ("Fire1") && counter > fireRate && !sprinting && rounds>0 && !reloading && !switching) {
			shoot ();
		}
		if (!reloading && !switching) {
			animator.SetBool ("grounded", grounded);
		}

		if (muzzleCounter > 0) {
			muzzleCounter += Time.deltaTime;
		}
		if (muzzleCounter > muzzleTime) {
			muzzleFlash.SetActive (false);
			muzzleCounter = 0;
		}
		if (Input.GetButtonDown ("Jump") && grounded) {
			if (sprinting) {
				animator.SetTrigger ("sprintJump");
				cameraAnimator.SetTrigger ("jump");
				skyboxCameraAnimator.SetTrigger ("jump");
			} else {
				animator.SetTrigger ("jump");
				cameraAnimator.SetTrigger ("jump");
				skyboxCameraAnimator.SetTrigger ("jump");
			}
		}
		if (Input.GetButtonDown ("Jump") && !grounded && !dblJump) {
			cameraAnimator.SetTrigger ("jump");
			skyboxCameraAnimator.SetTrigger ("jump");
		}
		cameraAnimator.SetBool ("grounded", grounded);
		skyboxCameraAnimator.SetBool ("grounded", grounded);
	}

	void shoot(){
		rounds--;

		//Animate the shot
		animator.SetTrigger("fire");
		cameraAnimator.SetTrigger ("shoot");
		skyboxCameraAnimator.SetTrigger ("shoot");

		//muzzleFlashRot = Random.Range (0, 360);
		//muzzleFlash.transform.rotation = Quaternion.AngleAxis (muzzleFlashRot, Vector3.forward);
		muzzleFlash.SetActive (true);
		muzzleCounter += Time.deltaTime;
		recoilCounter += Time.deltaTime;

		//Reset the counter
		counter = 0;
		//Set the bullet direction in the direction the camera is facing and add a bit of inaccuracy in there (poorly named variable. Whatever.)
		Vector3 bulletDirection = Camera.main.transform.forward;
		//bulletDirection.x += Random.Range (-accuracy, accuracy);
		//bulletDirection.y += Random.Range (-accuracy, accuracy);

		//Use a raycast for this weapon.
		RaycastHit hit;
		//Define the raycast from the bulletspawn, in  the direction the camera is facing.
		Ray ray = new Ray (bulletSpawn.transform.position, bulletDirection);
		Debug.DrawRay (bulletSpawn.transform.position, bulletDirection*100f); //Used for debugging
		//If the raycast hit something within 1000 units...
		if (Physics.Raycast (ray, out hit, 1000f)) {
			//And it hit something called an enemy...
			if (hit.collider.tag == "enemy") {
				hit.collider.gameObject.GetComponent<enemy01> ().health = hit.collider.gameObject.GetComponent<enemy01> ().health - damage;
			}
		}

		playerBullet = objectPooler.SharedInstance.GetPooledObject ("playerBullet");
		if (playerBullet != null) {
			playerBullet.transform.position = bulletSpawn.transform.position;
			playerBullet.transform.rotation = bulletSpawn.transform.rotation;
			playerBullet.SetActive (true);
		}
		annihilatorAudio.pitch=1;
		annihilatorAudio.clip = fireAudio;
		annihilatorAudio.Play ();
	}

	void reloadMechanics(){
		animator.SetTrigger("reload");
		reloading=true;
		reloadCounter+=Time.deltaTime;
		annihilatorAudio.pitch = 2;
		annihilatorAudio.clip = reloadAudio;
		annihilatorAudio.Play ();
		AimDownSights.canADS = false;
	}

	//A method to reload the bullets in the mag. Reset the timer, make the rounds in the weapon the magazine size, and set reloading to false.
	void reload(){
		reloadCounter = 0;
		rounds = magSize;
		reloading = false;
		counter = fireRate;
	}
}
