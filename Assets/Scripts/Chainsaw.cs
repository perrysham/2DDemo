using UnityEngine;
using System.Collections;

public class Chainsaw : MonoBehaviour {

	public float fuel = 100;				// Fuel of the chainsaw
	private bool rev;						// Bool for if the chainsaw if being revved

	void Update () 
	{
		//Find and update the in game fuel GUI
		GameObject.Find ("Fuel").guiText.text = "Fuel: " + fuel + " L";

		// If the fire button is pressed
		if (CFInput.GetButton ("Fire2")) 
		{	
			// Play chainsaw audio
			audio.Play ();

			// Set the chainsaw bool to hurt
			rev = true;

			// Decrement the fuel level
			fuel = fuel - 0.25f;
		} 
		else 
		{
			// If button is not being held
			rev = false;
		}
	}

	void OnTriggerEnter2D (Collider2D col) 
	{
		// If it hits an enemy and the fire button is being held
		if(col.tag == "Enemy1" && rev)
		{
			// Hurt the enemy
			col.gameObject.GetComponent<Enemy1>().Hurt();
		}
	}	
}
