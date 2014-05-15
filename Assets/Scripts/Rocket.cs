using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour 
{
	public GameObject explosion;		// Explosion effect prefab

	void Start () 
	{
		// Destroy rocket after 2 seconds if it doesn't hit an enemy or collide with environment
		Destroy(gameObject, 2);
	}
	
	void OnExplode()
	{
		// Instantiate explosion where the rocket has hit
		Instantiate(explosion, transform.position, transform.rotation);
	}
	
	void OnTriggerEnter2D (Collider2D col) 
	{
		// If it hits enemy1
		if(col.tag == "Enemy")
		{
			// Get Enemy script and call the Hurt function
			col.gameObject.GetComponent<Enemy>().Hurt();

			// Call the explosion function
			OnExplode();

			// Destroy rocket
			Destroy (gameObject);
		}

		// If it hits enemy2
		if(col.tag == "Enemy1")
		{
			// Get Enemy script and call the Hurt function
			col.gameObject.GetComponent<Enemy1>().Hurt();
			
			// Call the explosion function
			OnExplode();
		
			// Destroy rocket
			Destroy (gameObject);
		}

		// If it hits a weapon pickup
		else if(col.tag == "Pickup")
		{
			// Destroy the crate
			Destroy (col.transform.root.gameObject);

			// Destroy the rocket
			Destroy (gameObject);
		}
	}
}