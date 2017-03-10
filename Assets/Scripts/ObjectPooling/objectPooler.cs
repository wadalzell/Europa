using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectPooler : MonoBehaviour {

	[System.Serializable]
	public class ObjectPoolItem
	{
		public GameObject objectToPool;
		public int amountToPool;
		public bool shouldExpand;
	}

	public static objectPooler
		SharedInstance;

	public List<GameObject> pooledObjects;
	public List<ObjectPoolItem> itemsToPool;

	// Use this for initialization
	void Start () {
		pooledObjects = new List<GameObject> ();
		foreach (ObjectPoolItem item in itemsToPool) {
			for (int i = 0; i < item.amountToPool; i++) {
				GameObject obj = (GameObject)Instantiate (item.objectToPool);
				obj.SetActive (false);
				pooledObjects.Add (obj);
			}	
		}
	}

	void Awake(){
		SharedInstance = this;
	}

	public GameObject GetPooledObject(string tag){
		for (int i = 0; i < pooledObjects.Count; i++) {
			if (!pooledObjects [i].activeInHierarchy && pooledObjects [i].tag == tag) {
				return pooledObjects [i];
			}
		}
		foreach (ObjectPoolItem item in itemsToPool) {
			if (item.objectToPool.tag == tag) {
				if (item.shouldExpand) {
					GameObject obj = (GameObject)Instantiate (item.objectToPool);
					obj.SetActive (false);
					pooledObjects.Add (obj);
					return obj;
				}
			}
		}
		return null;
	}
}
