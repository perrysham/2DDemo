using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public float moveSpeed = 2f;		// Speed the enemy moves at
	public int HP = 2;					// Enemy hit points 
	public GameObject hundredPointsUI;	// A prefab of 100 that appears when the enemy dies
	private SpriteRenderer ren;			// Reference to the sprite renderer
	private Transform frontCheck;		// Reference to the gameobject used to check if somethings in front
	private bool dead = false;			// Whether or not the enemy is dead
	private Score score;				// Reference to the Score script

	
	void Awake()
	{
		// Setting up the references
		// Find the frontcheck position of the gameobject
		frontCheck = transform.Find("frontCheck").transform;

		// Find the score script
		score = GameObject.Find("Score").GetComponent<Score>();
	}

	void FixedUpdate ()
	{
		// Create array of colliders in front of the enemy
		Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);

		// Check each of the colliders
		foreach(Collider2D c in frontHits)
		{
			// If any of the colliders is an obstacle
			if(c.tag == "Obstacle")
			{
				// Flip the enemies orientation
				Flip ();

				// Set the enemy move speed to 0
				moveSpeed = 0;

				// Call the jump function
				Jump ();

				// After applying jump reset the move speed
				moveSpeed = 1.0f;
			}

			if(c.tag == "Flip")
			{
				// Flip the enemies orientation
				Flip ();
			}

			if(c.tag == "Chainsaw")
			{
				// Call the hurt function
				Hurt();
			}

			// Ignore layer collision with other form of enemy
			Physics2D.IgnoreLayerCollision(10,15);

			// Ignore layer collision with self
			Physics2D.IgnoreLayerCollision(15,15);

			// Ignore layer collision the player itself
			Physics2D.IgnoreLayerCollision(9,15);
		}

		// Set the enemy velocity to move
		rigidbody2D.velocity = new Vector2(transform.localScale.x * moveSpeed, rigidbody2D.velocity.y);	
			
		// If enemy is dead by hasnt died for some reason
		if(HP <= 0 && !dead)
			// Call death function
			Death ();
	}
	
	public void Hurt()
	{
		// Reduce enemies health by 1
		HP--;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		//
		if(collision.collider.tag == "NormalSpeed")
		{
			moveSpeed = 3.0f;
		}

		//
		if(collision.collider.tag == "BigJump")
		{
			Jump1 ();
		}
	}

	void Jump()
	{
		// Enable enemy to jump
		rigidbody2D.AddForce(new Vector2(0f, 575.0f));
	}

	void Jump1()
	{
		// Enable higher jump
		rigidbody2D.AddForce(new Vector2(0f, 350.0f));
	}
	
	void Death()
	{
		// Find all of the sprite renderers and childrens 
		SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

		// Disable all sprite renderers
		foreach(SpriteRenderer s in otherRenderers)
		{
			s.enabled = false;
		}

		// Increase the score by 100 points
		score.score += 100;

		// Set dead to true
		dead = true;

		// Find all of the colliders on the gameobject and set them all to be triggers
		Collider2D[] cols = GetComponents<Collider2D>();

		foreach(Collider2D c in cols)
		{
			c.isTrigger = true;
		}

		// Create a vector for the score above that is just above the enemy
		Vector3 scorePos;
		scorePos = transform.position;
		scorePos.y += 1.5f;

		// Create the amount of score above the player
		Instantiate(hundredPointsUI, scorePos, Quaternion.identity);
	}
	
	public void Flip()
	{
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;

		// Set the scale of enemy to the opposite direction
		transform.localScale = enemyScale;
	}
}