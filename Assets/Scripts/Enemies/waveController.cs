using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveController : MonoBehaviour {

	public int round=0;
	public int wave=1;
	public int set=1;

	public float roundMod=0.5f;

	public float timer;
	public float intermission = 30f;

	public int initialEnemyAmount = 20;
	public int enemyToSpawn = 10;
	public int enemyLeft;

	public GameObject player;

	GameObject[] sp01;

	private healthController HealthController;

	// Use this for initialization
	void Start () {
		sp01 = GameObject.FindGameObjectsWithTag ("sp01");
		increaseRound ();
		player = GameObject.FindGameObjectWithTag ("Player");
		HealthController = player.GetComponent<healthController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (enemyToSpawn > 0) {
			foreach (GameObject sp in sp01) {
				sp.SetActive (true);
			}
		} else {
			foreach (GameObject sp in sp01) {
				sp.SetActive (false);
			}
		}
			
		if (enemyLeft <= 0 && enemyToSpawn <= 0 && timer == 0) {
			round++;
			timer += Time.deltaTime;
		}
		if (timer > 0) {
			timer += Time.deltaTime;
		}
		if (enemyLeft <= 0 && enemyToSpawn <= 0 && timer > intermission) {
			spawnEnemy ();
		}

		HealthController.round = round;
		HealthController.wave = wave;
		HealthController.set = set;
	}

	void spawnEnemy(){
		enemyToSpawn = (int)Mathf.Ceil (initialEnemyAmount * round * wave * set * roundMod);
		enemyLeft = enemyToSpawn;
		timer = 0f;
	}

	void increaseRound(){
		round++;
		timer = 0;
	}
}
