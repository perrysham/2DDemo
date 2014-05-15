using UnityEngine;


[AddComponentMenu("ControlFreak-Demos-CS/GunCS")]
public class GunCS : MonoBehaviour 
{
public ParticleSystem 	shotParticles;

public AudioClip		shotSound;
	
public AudioClip		emptySound;

public AudioClip		reloadSound;

private bool			isFiring;	
public float			shotInterval = 0.175f;	
	
private float			lastShotTime;	
	
public bool				unlimitedAmmo	= false;
public int 				bulletCapacity 	= 40,
						bulletCount		= 40;

public Transform		projectileOrigin;		// Transform where projectiles will be fired from.
public BulletCS			bulletPrefab;			// Bullet prefab reference. 


// --------------------
private void Start()
	{
	this.isFiring = false;
	}


// -----------------------
public void SetTriggerState(bool fire)
	{	
	if (fire != this.isFiring)
		{
		this.isFiring = fire;
		
		if (fire)
			{
			// Fire first bullet...

			this.FireBullet();	
			}
		else
			{
			}
		}
	}

	

// --------------------
private void FixedUpdate()
	{
//#if UNITY_EDITOR
//this.SetTriggerState(Input.GetKey(KeyCode.F));
//#endif

	if (this.isFiring)
		this.FireBullet();
	
	}

 

// ------------------	
public void Reload()
	{
	this.bulletCount = this.bulletCapacity;
	
	if ((this.audio != null) && (this.reloadSound != null))
		{
		this.audio.loop = false;
		this.audio.PlayOneShot(this.reloadSound);
		}
	}


// ---------------------
private void FireBullet()
	{
	if ((Time.time - this.lastShotTime) >= this.shotInterval)
		{
		this.lastShotTime = Time.time;
	

		// Shoot...
			
		
		if (this.unlimitedAmmo || (this.bulletCount > 0))
			{
			if (!this.unlimitedAmmo)
				--this.bulletCount;	

			// Emit particles...
				
			if ((this.shotParticles != null) )
				{
				this.shotParticles.Play();
				}
	
			// Fire projectile.
	
			if ((this.projectileOrigin != null) && (this.bulletPrefab != null))
				{
				BulletCS bullet = Instantiate(this.bulletPrefab, 
					this.projectileOrigin.position, this.projectileOrigin.rotation) as BulletCS;

				if (bullet != null)
					bullet.Init(this); 
				} 

	

			// Play sound...
	
			if ((this.audio != null) && (this.shotSound != null)) // && (!this.shotSoundLooped))
				{	
				this.audio.loop = false;
				this.audio.PlayOneShot(this.shotSound);	
				}
			}

		// No bullets left!!

		else
			{
			// Play sound...
	
			if ((this.audio != null) && (this.emptySound != null)) // && (!this.emptySoundLooped))
				{	
				this.audio.loop = false;
				this.audio.PlayOneShot(this.emptySound);	
				}
			}

		}
	}

}
