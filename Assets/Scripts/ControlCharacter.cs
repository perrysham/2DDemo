using UnityEngine;
using System.Collections;

public class ControlCharacter : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.

	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.


	private bool grounded = false;	
	private Transform groundCheck;	
	private Transform chainCheck;

	private GunPickup picks;

	private Animator anim;					// Reference to the player's animator component.


	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		chainCheck = transform.Find("chainCheck").transform;
		anim = GetComponent<Animator>();
		gameObject.GetComponentInChildren<CircleCollider2D> ().enabled = false;
	}

	void Start(){

		var obj1 = GameObject.Find ("puggun2");
		obj1.renderer.enabled = false;
		var obj2 = GameObject.Find ("chainsaw");
		obj2.renderer.enabled = false;

	}
		
	void Update()
	{

		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")); 
		
		// If the jump button is pressed and the player is grounded then the player should jump.
		if(CFInput.GetButtonDown("Jump") && grounded)
			jump = true;

		Physics2D.IgnoreLayerCollision(9,14);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.collider.tag == "Ladder")
		{
			jump = false;
			jumper ();
		}
	}
	
	void jumper()
	{
		rigidbody2D.AddForce(new Vector2(0f, 1200.0f));
	}

	void FixedUpdate ()
	{
				//if (networkView.isMine) {
						// Cache the horizontal CFInput.
						float h = CFInput.GetAxis ("Horizontal");
		
						// The Speed animator parameter is set to the absolute value of the horizontal CFInput.
						anim.SetFloat ("Speed", Mathf.Abs (h));
		
						// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
						if (h * rigidbody2D.velocity.x < maxSpeed)
			// ... add a force to the player.
								rigidbody2D.AddForce (Vector2.right * h * moveForce);
		
						// If the player's horizontal velocity is greater than the maxSpeed...
						if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
								rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		
						// If the input is moving the player right and the player is facing left...
						if (h > 0 && !facingRight)
			// ... flip the player.
								Flip ();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (h < 0 && facingRight)
			// ... flip the player.
								Flip ();
		
						// If the player should jump...
						if (jump) {
								// Set the Jump animator trigger parameter.
								//anim.SetTrigger("Jump");
			
								// Play a random jump audio clip.
								//int i = Random.Range(0, jumpClips.Length);
								//AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);
			
								// Add a vertical force to the player.
								rigidbody2D.AddForce (new Vector2 (0f, jumpForce));
			
								// Make sure the player can't jump again until the jump conditions from Update are satisfied.
								jump = false;
						}
				//}
		}
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void changeGun(){

		int choice = Random.Range (2, 4);
		print (choice);
		if (choice == 2) {
			print("gun2");
			var gun1 = GameObject.Find ("pug_gun");
			gun1.renderer.enabled = false;
			gameObject.GetComponentInChildren<Gun> ().enabled = false;
			var gun2 = GameObject.Find ("puggun2");
			gun2.renderer.enabled = true;
			gameObject.GetComponentInChildren<Gun2> ().enabled = true;
			//return;
		}
		if (choice == 3) {
			print("gun3");
			var gun1 = GameObject.Find ("pug_gun");
			gun1.renderer.enabled = true;
			gameObject.GetComponentInChildren<Gun> ().enabled = true;
			var gun2 = GameObject.Find ("puggun2");
			gun2.renderer.enabled = false;
			gameObject.GetComponentInChildren<Gun2> ().enabled = false;
			//return;
		}
	}

	public void specialPower(){

		var gun1 = GameObject.Find ("pug_gun");
		gun1.renderer.enabled = false;
		gameObject.GetComponentInChildren<Gun> ().enabled = false;
		var gun2 = GameObject.Find ("puggun2");
		gun2.renderer.enabled = false;
		gameObject.GetComponentInChildren<Gun2> ().enabled = false;
		var chain = GameObject.Find ("chainsaw");
		chain.renderer.enabled = true;
		gameObject.GetComponentInChildren<Chainsaw> ().enabled = true;
		gameObject.GetComponentInChildren<CircleCollider2D> ().enabled = true;
	}
}
