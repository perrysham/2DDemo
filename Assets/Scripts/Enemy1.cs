using UnityEngine;
using System.Collections;

public class Enemy1 : MonoBehaviour
{
	public float moveSpeed = 2f;		// The speed the enemy moves at.
	public int HP = 2;					// How many times the enemy can be hit before it dies.
	public GameObject hundredPointsUI;	// A prefab of 100 that appears when the enemy dies.
	private SpriteRenderer ren;			// Reference to the sprite renderer.
	private Transform frontCheck;		// Reference to the position of the gameobject used for checking if something is in front.
	private bool dead = false;			// Whether or not the enemy is dead.
	private Score score;				// Reference to the Score script.
	
	
	void Awake()
	{
		// Setting up the references
		frontCheck = transform.Find("frontCheck").transform;
		score = GameObject.Find("Score").GetComponent<Score>();
	}
	
	void FixedUpdate ()
	{
		// Create an array colliders in front of the enemy
		Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, 1);
		
		// Check each of the colliders.
		foreach(Collider2D c in frontHits)
		{
			// If any of the colliders is an Obstacle...
			if(c.tag == "Obstacle")
			{
				// ... Flip the enemy and stop checking the other colliders.
				Flip ();
				moveSpeed = 0;
				//rigidbody2D.AddForce(new Vector2(0f, 10.0f));
				jump ();
				moveSpeed = 1.0f;
				
				//break;
			}

			if(c.tag == "Flip")
			{
				// ... Flip the enemy and stop checking the other colliders.
				Flip ();
			}

			Physics2D.IgnoreLayerCollision(10,15);
			Physics2D.IgnoreLayerCollision(10,10);
		}
		
		// Set the enemy's velocity to moveSpeed in the x direction.
		rigidbody2D.velocity = new Vector2(transform.localScale.x * moveSpeed, rigidbody2D.velocity.y);	
		
		// If the enemy has one hit point left and has a damagedEnemy sprite...
		//if(HP == 1 && damagedEnemy != null)
		// ... set the sprite renderer's sprite to be the damagedEnemy sprite.
		//	ren.sprite = damagedEnemy;
		
		// If the enemy has zero or fewer hit points and isn't dead yet...
		if(HP <= 0 && !dead)
			// ... call the death function.
			Death ();
	}
	
	public void Hurt()
	{
		// Reduce the number of hit points by one.
		HP--;
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.collider.tag == "NormalSpeed")
		{
			moveSpeed = 3.0f;
		}
		if(collision.collider.tag == "BigJump")
		{
			//moveSpeed = 0;
			//moveSpeed = 1.0f;
			jump1 ();
			
			//break;
		}
	}
	
	void jump()
	{
		rigidbody2D.AddForce(new Vector2(0f, 575.0f));
	}
	
	void jump1()
	{
		rigidbody2D.AddForce(new Vector2(0f, 500.0f));
		print ("has jumped1");
		//Physics2D.IgnoreLayerCollision (10, 14);
	}
	
	void Death()
	{
		// Find all of the sprite renderers on this object and it's children.
		SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();
		
		// Disable all of them sprite renderers.
		foreach(SpriteRenderer s in otherRenderers)
		{
			s.enabled = false;
		}
		
		// Re-enable the main sprite renderer and set it's sprite to the deadEnemy sprite.
		//		ren.enabled = true;
		//ren.sprite = deadEnemy;
		
		// Increase the score by 100 points
		score.score += 100;
		//print (score.score);
		
		// Set dead to true.
		dead = true;
		
		// Allow the enemy to rotate and spin it by adding a torque.
		rigidbody2D.fixedAngle = false;
		//rigidbody2D.AddTorque(Random.Range(deathSpinMin,deathSpinMax));
		
		// Find all of the colliders on the gameobject and set them all to be triggers.
		Collider2D[] cols = GetComponents<Collider2D>();
		foreach(Collider2D c in cols)
		{
			c.isTrigger = true;
		}
		
		// Play a random audioclip from the deathClips array.
		//int i = Random.Range(0, deathClips.Length);
		//AudioSource.PlayClipAtPoint(deathClips[i], transform.position);
		
		// Create a vector that is just above the enemy.
		Vector3 scorePos;
		scorePos = transform.position;
		scorePos.y += 1.5f;
		
		// Instantiate the 100 points prefab at this point.
		Instantiate(hundredPointsUI, scorePos, Quaternion.identity);
	}
	
	
	public void Flip()
	{
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}
}

/*using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	public Transform target;
	Transform enemyTransform;
	public float speed = 3f;
	public float rotationSpeed=3f;
	public int HP = 2;
	Vector3 upAxis = new Vector3 (0f, 0f, -1f);
	bool fliper = false;

	void Start () {
		//obtain the game object Transform
		enemyTransform = this.GetComponent<Transform>();
	}
	
	void Update(){
		
		//target = GameObject.FindWithTag ("Player").transform;
		//Vector3 targetHeading = target.position - transform.position;
		//Vector3 targetDirection = targetHeading.normalized;

		if (transform.position.x <= target.transform.position.x) {

			if(fliper){

				rigidbody2D.velocity = new Vector2(transform.localScale.x * -3, rigidbody2D.velocity.y);
			}
		}

		if (transform.position.x >= target.transform.position.x) {
			rigidbody2D.velocity = new Vector2(transform.localScale.x * 3, rigidbody2D.velocity.y);
		}

		//rigidbody2D.velocity = new Vector2(transform.localScale.x * 3, rigidbody2D.velocity.y);

		if (fliper) {
			// Multiply the x component of localScale by -1.
			Vector3 enemyScale = transform.localScale;
			enemyScale.x *= -1;
			transform.localScale = enemyScale;
			fliper = false;
		}
		
		//rotate to look at the player
		
		//transform.rotation = Quaternion.LookRotation(targetDirection); // Converts target direction vector to Quaternion
		//Flip();
		//transform.eulerAngles = new Vector3(-1, transform.eulerAngles.y, 0);

		//transform.LookAt (target.position, upAxis);
		//transform.eulerAngles = new Vector3 (0f, 0f, transform.eulerAngles.z);
		
		//move towards the player
		//enemyTransform.position += enemyTransform.forward * speed * Time.deltaTime;
		//rigidbody2D.velocity = new Vector2(transform.localScale.x * 3, rigidbody2D.velocity.y);
		//rigidbody2D.transform.position += rigidbody2D.transform.forward * speed * Time.deltaTime;
		
	}

	public void Flip()
	{
		// Multiply the x component of localScale by -1.
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
		fliper = false;
	}

	public void Hurt()
	{
		// Reduce the number of hit points by one.
		//HP--;
	}
	
}*/
