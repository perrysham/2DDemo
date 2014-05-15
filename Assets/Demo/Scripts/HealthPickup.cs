using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour
{
	private float healthBonus = 25.0f;				// How much health the crate gives the player.
	public AudioClip collect;				// The sound of the crate being collected.
	
	void OnTriggerEnter2D (Collider2D other)
	{
		// If the player enters the trigger zone...
		if(other.tag == "Player")
		{
			// Reference the player health script
			PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

			// Increasse the health of the player but clamp it at 100
			playerHealth.health += healthBonus;
			playerHealth.health = Mathf.Clamp(playerHealth.health, 0f, 100f);

			// Update the health bar
			playerHealth.UpdateHealthBar();

			// Play the collection sound
			//AudioSource.PlayClipAtPoint(collect,transform.position);

			// Destroy the crate.
			Destroy(transform.root.gameObject);
		}
	}
}