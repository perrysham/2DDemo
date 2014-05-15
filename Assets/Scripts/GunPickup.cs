using UnityEngine;
using System.Collections;

public class GunPickup : MonoBehaviour
{
	//public float healthBonus;				// How much health the crate gives the player.
	//public AudioClip collect;				// The sound of the crate being collected.
	
	
	//private PickupSpawner pickupSpawner;	// Reference to the pickup spawner.
	//private Animator anim;					// Reference to the animator component.
	//private bool landed;					// Whether or not the crate has landed.
	
	
	void Awake ()
	{
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		// If the player enters the trigger zone...
		if(other.tag == "Player")
		{
			// Get a reference to the player health script.
			ControlCharacter player = other.GetComponent<ControlCharacter>();
			
			// Increasse the player's health by the health bonus but clamp it at 100.
			player.changeGun();

			SpawnNew();
			
			// Play the collection sound.
			//AudioSource.PlayClipAtPoint(collect,transform.position);
			
			// Destroy the crate.
			Destroy(transform.root.gameObject);
		}
		// Otherwise if the crate hits the ground...
		else if(other.tag == "Ground")
		{	
			//transform.parent = null;
			//gameObject.AddComponent<Rigidbody2D>();

		}
	}

	public void SpawnNew(){

		int choice = Random.Range (1, 4);

		if (choice == 1) {

			Vector3 pos = new Vector3 (12, -7.1f,0);
			Instantiate(gameObject, pos, Quaternion.identity);
		}

		if (choice == 2) {
			
			Vector3 pos = new Vector3 (-33, -3.5f,0);
			Instantiate(gameObject, pos, Quaternion.identity);
		}

		if (choice == 3) {
			
			Vector3 pos = new Vector3 (-23, 0.9f,0);
			Instantiate(gameObject, pos, Quaternion.identity);
		}

		if (choice == 4) {
			
			Vector3 pos = new Vector3 (0, -1.8f,0);
			Instantiate(gameObject, pos, Quaternion.identity);
		}
	}
}
