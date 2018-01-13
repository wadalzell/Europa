using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponTurn : MonoBehaviour {

	public GameObject weaponHolder;

	public float moveAmt = 4f;
	public float rotSpeed = 3f;
	public float moveLR;
	public float moveUD;
	public Vector3 defaultPos;
	public Vector3 nextPos;

	// Use this for initialization
	void Start () {
		defaultPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		moveLR = Input.GetAxis ("Mouse X") *Time.deltaTime* moveAmt;
		moveUD = Input.GetAxis ("Mouse Y") *Time.deltaTime * moveAmt;
		nextPos = new Vector3 (defaultPos.x + moveLR, defaultPos.y + moveUD, defaultPos.z);
		weaponHolder.transform.localPosition = Vector3.Lerp (weaponHolder.transform.localPosition, nextPos, rotSpeed * Time.deltaTime);
	}
}
