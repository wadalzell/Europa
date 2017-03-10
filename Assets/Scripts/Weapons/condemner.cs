using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class condemner : MonoBehaviour {

	public GameObject bulletSpawn;

	public float fireRate = 0.5f;
	public float damage = 25f;
	private float counter = 0;
	public float accuracy = 0.02f;
	public float speed;
	public float reloadTime = 3f;
	public int magSize = 10;
	public int rounds;

	public GameObject weaponModel;
	public GameObject playerBullet;
	public Animator animator;

	public AudioSource condemnerAudio;
	public AudioClip fireAudio;
	public AudioClip reloadAudio;

	public bool reloading;

	bool sprinting;
	float reloadCounter;


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
		condemnerAudio = GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {

		speed = player.GetComponent<Rigidbody> ().velocity.magnitude;
		animator.SetFloat ("speed", speed);

		sprinting = player.GetComponent<playerMove> ().sprinting;

		//If the fire button is pressed and there has been enough time since the last shot was fired, and we're not sprinting, and were not out of bullets, then shoot!
		if (Input.GetButtonDown ("Fire1") && counter > fireRate && !sprinting && rounds>0) {
			shoot ();
		}
		//Update the counter.
		counter +=Time.deltaTime;


		//This next section handles the reloading. If the player tries to shoot, but is out of bullets, and is not already reloading, start the reload anim, set reload to
		//true and begin the reload timer.
		if (Input.GetButtonDown ("Fire1") && rounds == 0 && !reloading) {
			animator.SetTrigger("reload");
			reloading=true;
			reloadCounter+=Time.deltaTime;
			condemnerAudio.clip = reloadAudio;
			condemnerAudio.Play ();
		}
			
		//This works the same as the if statement above, but sees if the player pressed the reload button and is not reloading.
		if(Input.GetKeyDown(KeyCode.R) && !reloading){
			animator.SetTrigger("reload");
			reloading=true;
			reloadCounter+=Time.deltaTime;
			condemnerAudio.clip = reloadAudio;
			condemnerAudio.Play ();
		}
		//If the reload timer has been started, continue it. This is for animating purposes.
		if(reloadCounter>0){
			reloadCounter+=Time.deltaTime;
		}
		//If the weapon is reloading, and the reload timer is greater than the reload time (checking if the animation is complete) then actually reload the bullets.
		if(reloading && reloadCounter>reloadTime){
			reload();
		}
	}

	void shoot(){
		rounds--;

		//Animate the shot
		animator.SetTrigger("fire");

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

		condemnerAudio.clip = fireAudio;
		condemnerAudio.Play ();
	}

	//A method to reload the bullets in the mag. Reset the timer, make the rounds in the weapon the magazine size, and set reloading to false.
	void reload(){
		reloadCounter = 0;
		rounds = magSize;
		reloading = false;
	}
}
