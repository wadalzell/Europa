using UnityEngine;
using System.Collections;
[RequireComponent (typeof(CharacterController))]

public class playerMove : MonoBehaviour {

	public float speedModifier=8;
	public float sprintModifier=1f;
	public float rotModifier=6;
	public float rotLimit = 90.0f;
	public float jumpSpeed = 10f;
	public float jumpCounter = 0.0f;
	public float jumpRate = 1f;
	public float gForce = 2.5f;
	public float velocityMag;
	public float verticalVelocityMag;
	public float maxVelocityChange = 10.0f;

	public AudioSource playerAudio;
	public AudioClip playerRunAudio;

	public float verticalRotation = 0;
	float rotLR = 0;
	float groundRadius = 0.75f;

	public bool dblJump = false;
	public bool grounded = false;
	public bool sprinting = false;
	public bool landing = false;

	public Transform groundCheck;

	public LayerMask whatIsGround;

	public Rigidbody rb;

	public Camera weaponCamera;
	public GameObject cameraHolder;
	public GameObject weaponHolder;
	Animator weaponHolderAnimator;
	Animator cameraHolderAnimator;

	// Use this for initialization
	void Start () {
		//Remove cursor and set components
		Cursor.visible=false;
		rb = GetComponent<Rigidbody> ();
		playerAudio = GetComponent<AudioSource> ();
		weaponHolderAnimator = weaponHolder.GetComponent<Animator> ();
		cameraHolderAnimator = cameraHolder.GetComponent<Animator> ();
	}

	void FixedUpdate(){
		//Check if player is grounded. If he is, he has not double jumped.
		grounded = Physics.CheckSphere (groundCheck.position, groundRadius, whatIsGround);
		if (grounded) {
			dblJump = false;
			weaponHolderAnimator.SetBool ("doubleJump", dblJump);
		}
	}

	void Update(){
		//Used to check FPS of game for debug. Remove in final version
		//float FPS = 1 / Time.deltaTime;

		//Take mouse x axis and transform the player by this amount.
		rotLR = Input.GetAxis ("Mouse X")*rotModifier;
		transform.Rotate (0, rotLR, 0);

		//For the mouse y axis (vertical,) the input must be "clamped" between a +- value (rotLimit,) else the player can rotate vertically indefinitly.
		//Also, the camera is transformed; not the player, so looking up does not throw the player on his back.
		verticalRotation -=Input.GetAxis ("Mouse Y")*rotModifier;
		verticalRotation = Mathf.Clamp (verticalRotation, -rotLimit, rotLimit);
		Camera.main.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);
		weaponCamera.transform.localRotation = Quaternion.Euler (verticalRotation, 0, 0);

		//Jumping code. On the jump botton, if they are grounded, they can jump. If they are not grounded and have not double jumped, they can jump once more  for
		//6a double jump. If they have jumped, are not grounded, and have double jumped, then nothing happens..

		jumpCounter += Time.deltaTime;
		if (Input.GetButtonDown ("Jump") && grounded && jumpCounter > jumpRate) {
			rb.AddForce (Vector3.up * jumpSpeed, ForceMode.Impulse);
			weaponHolderAnimator.SetTrigger ("jump");
			cameraHolderAnimator.SetTrigger ("jump");
			jumpCounter = 0;
		}
		if (Input.GetButtonDown ("Jump") && !grounded && !dblJump && jumpCounter > jumpRate) {
			rb.velocity = new Vector3 (0, 0, 0);
			rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
			dblJump = true;
			jumpCounter = 0;
			weaponHolderAnimator.SetTrigger ("jump");
			weaponHolderAnimator.SetBool ("doubleJump", dblJump);
			cameraHolderAnimator.SetTrigger ("jump");
		}


		//This bit makes jumping feel more lifelike. It dampens the upward jumping while allows the falling to occur at normal conditions.
		if (rb.velocity.y > 0) {
			rb.drag = 2.0f;
		} else {
			rb.drag = 0.0f;
		}


		//If sprinting, increase speed by the spring modifier
		if(Input.GetButton("sprint") && Input.GetAxis("Vertical")>0){
			sprintModifier = 1.5f;
			sprinting = true;
			playerAudio.pitch = 1.5f;
			//GetComponent<aimDownSights> ().sprinting = true;
		}else{
			sprintModifier = 1f;
			sprinting = false;
			playerAudio.pitch = 1;
			//GetComponent<aimDownSights> ().sprinting = false;
		}

		//Player movement code. Creates a vector in the x direction on the horizontal axis (a/d) andi n the z direction (local) on the vertical axis (w/s.)
		//The vector is then assigned as a transform and multiplied by the speed modifier of the player.
		Vector3 targetMove = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		targetMove = transform.TransformDirection (targetMove);
		targetMove *= speedModifier*sprintModifier;

		//A rigidbody velocity is used to actually control the player, and the velocity change (clamped by maxVelocityChange) is applied as a force to the player's rb.
		Vector3 velocity = rb.velocity;
		Vector3 velocityChange = (targetMove - velocity);
		velocityChange.x = Mathf.Clamp (velocityChange.x, -maxVelocityChange, maxVelocityChange);
		velocityChange.z = Mathf.Clamp (velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = 0;
		rb.AddForce (velocityChange, ForceMode.VelocityChange);
		//No clue why this is here. Remove?
		velocityMag = rb.velocity.magnitude;
		if (velocityMag > 1 && grounded) {
			if (playerAudio.isPlaying == false) {
				playerAudio.clip = playerRunAudio;
				playerAudio.Play ();
			}
		} else {
			playerAudio.Stop ();
		}

		if (!grounded) {
			landing = true;
		}

		if (grounded && landing) {
			cameraHolderAnimator.SetTrigger ("land");
			weaponHolderAnimator.SetTrigger ("land");
			landing = false;
		}
	}
}
