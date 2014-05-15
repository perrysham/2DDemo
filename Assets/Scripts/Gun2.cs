using UnityEngine;
using System.Collections;

public class Gun2 : MonoBehaviour
{
	public Rigidbody2D bullet;				// Bullet prefab
	public float speed = 20f;				// Speed the rocket will fire at
	private ControlCharacter playerCtrl;	// Reference to the PlayerControl script

	void Awake()
	{
		// Setting up the references
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
				// Instantiate the bullet facing right and set the velocity to the right
				Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(speed, 0);
			}
			else
			{
				// Instantiate the bullet facing left and set the velocity to the left
				Rigidbody2D bulletInstance = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-speed, 0);
			}
		}
	}
}