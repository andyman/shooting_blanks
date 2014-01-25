using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public int player = -1; // index of player, starting with zero. Use -1 to mean it is not set yet.

	float expirationTime; // time when the bullet is too long lived

	// Use this for initialization
	void Start () {

		// set up the deathtime of the bullet
		expirationTime = Time.time + 5.0f;
	}
	
	// Update is called once per frame
	void Update () {

		// expire the bullet if it has lived too long
		if (Time.time > expirationTime)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		int layer = other.gameObject.layer;

		Debug.Log ("Trigger enter " + other.name);

		// check the layer
		if (LayerConstants.PlayerFlag == layer)
		{
			HandlePlayerHit(other);
		}
		else if (LayerConstants.ObstacleFlag == layer)
		{
			HandleObstacleHit(other);
		}

	}

	void HandlePlayerHit(Collider2D other)
	{

		Actor otherActor = other.GetComponent<Actor>();

		// ignore if there is no player, or it is our own
		if (otherActor == null || this.player == -1 || otherActor.player == this.player)
		{
			return;
		}



		otherActor.ApplyHit(this);

		// TODO make boom boom effect

		// remove this bullet
		Destroy(gameObject);
	}

	void HandleObstacleHit(Collider2D other)
	{
		// TODO
		// When projectile hits a wall, display an effect
		Debug.Log("TODO: Obstacle hit");

		// and then destroy or recycle the projectile
		Destroy(gameObject);
	}

}
