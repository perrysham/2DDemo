using UnityEngine;
using System.Collections;

public class SpecialPickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		// If the player enters the trigger zone...
		if(other.tag == "Player")
		{
			// Get a reference to the player health script.
			ControlCharacter player = other.GetComponent<ControlCharacter>();
			
			// Increasse the player's health by the health bonus but clamp it at 100.
			player.specialPower();
			
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
}
