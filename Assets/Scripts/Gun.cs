using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
	public Rigidbody2D rocket;				// Rocket prefab
	public float speed = 20f;				// Speed the rocket will fire at
	private ControlCharacter playerCtrl;	// Reference to the PlayerControl script

	void Awake()
	{
		// Setting up the reference
		playerCtrl = transform.root.GetComponent<ControlCharacter>();
	}

	void Update ()
	{
		// If the fire button is pressed
		if(CFInput.GetButtonDown("Fire2"))
		{
			// play audioclip
			audio.Play();

			// If the player is facing right
			if(playerCtrl.facingRight)
			{
				// Instantiate the rocket facing right and set the velocity to the right
				Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(speed, 0);
			}
			else
			{
				// Instantiate the rocket facing left and set the velocity to the left
				Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-speed, 0);
			}
		}
	}
}
