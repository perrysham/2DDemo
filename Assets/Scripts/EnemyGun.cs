using UnityEngine;
using System.Collections;

public class EnemyGun : MonoBehaviour
{
	public Rigidbody2D rocket;				// Prefab of the rocket.
	public float speed = 20f;				// The speed the rocket will fire at.
	private Animator anim;					// Reference to the Animator component.
	private float shootTimer;
	private float fireRate = 2.0f;
	private float lastShot = 0.0f;
	
	void Update ()
	{
		
		var player = GameObject.Find ("Player");
		var distance = Vector2.Distance(transform.position,player.transform.position);
		//print (distance);
		// If the fire button is pressed...
		if (distance <= 4){
			
			print("in range");
			fire ();
		}
	}
	
	public void fire(){
		
		if (Time.time > fireRate + lastShot) {
			
			Rigidbody2D bulletInstance = Instantiate(rocket, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			bulletInstance.velocity = new Vector2(speed, 0);
			lastShot = Time.time;
		}
	}
}
