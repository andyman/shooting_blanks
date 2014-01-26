using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
	
	public int player = 0; // index of the player, starting with 0
	
	public float rotationRate = 120.0f; // max angles per second that the clone can rotate
	public float speed = 5.0f; // units per second that the clone can move forward/back
	public float fireInterval = 1.0f; // how often we can fire a projectile
	public bool real = false; // whether we are the real thing
	public float projectileSpeed = 10.0f; // the speed of the bullet
	public Transform projectilePrefab = null; // the prefab for the bullet
	public Transform deathPrefab = null; // the corpse or death effect that is instantiated when this actor dies

	private float nextFireTime = 0.0f;
	
	// Use this for initialization
	void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {
		
		// left/right turning
		float horizontal = Input.GetAxis("Horizontal"+player);
		rigidbody2D.angularVelocity = -horizontal * rotationRate;
		
		// forward/back
		float vertical = Input.GetAxis("Vertical"+player);
		rigidbody2D.velocity = vertical * speed * transform.up;
		
		// fire
		if (Input.GetButton("Fire"+player) && Time.time >= nextFireTime && projectilePrefab != null)
		{
			Fire();
		}
	}
	
	// fires the projectile
	void Fire() {
		// set the next fire time
		nextFireTime = Time.time + fireInterval;
		
		// create a new projectile with the same position and rotation as me
		Transform bulletTransform = Instantiate(projectilePrefab, transform.position, transform.rotation) as Transform;

		// set the projectile's player (set to -1 if it is fake)
		Projectile projectile = bulletTransform.GetComponent<Projectile>();
		projectile.player = this.real ? player : -1;

		// set projectile in motion
		Rigidbody2D bulletPhysics = bulletTransform.GetComponent<Rigidbody2D>();
		Vector3 myUpVector = transform.up;
		bulletPhysics.velocity = rigidbody2D.velocity + projectileSpeed * new Vector2(myUpVector.x, myUpVector.y);
		
	}

	public void ApplyHit(Projectile projectile)
	{
		// create a corpse
		if (deathPrefab != null)
		{
			Instantiate(deathPrefab, transform.position+(new Vector3(0,0,-5.0f)), transform.rotation);
		}
		
		// report to the level controller so it can do its logic
		LevelController.instance.ReportActorDeath(this);
		
		// remove this
		Destroy(this.gameObject);
	}
	
	public void ApplyHit(Hazard hazard)
	{
		// create a corpse
		if (deathPrefab != null)
		{
			Instantiate(deathPrefab, transform.position, transform.rotation);
		}
		
		// report to the level controller so it can do its logic
		LevelController.instance.ReportActorDeath(this);
		
		// remove this
		Destroy(this.gameObject);
	}
	public void ApplyPowerUp(PowerUp powerUp)
	{
		LevelController.instance.ApplyPowerUp(this);
	}
}
