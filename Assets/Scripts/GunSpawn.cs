using UnityEngine;
using System.Collections;

public class GunSpawn : MonoBehaviour {

	public GameObject gunPickup;
	private int numSpawned;
	private int spawned;

	// Use this for initialization
	void Start () {

		numSpawned = 5;
	}
	
	// Update is called once per frame
	void Update () {

		spawned = 0;

		while(spawned < numSpawned) {

			print ("spawned");
			Vector2 pos = new Vector2 (Random.Range(-35, 25), Random.Range(-5, 5));
			Instantiate(gunPickup, pos, Quaternion.identity);
			spawned++;

		}
	}
}
