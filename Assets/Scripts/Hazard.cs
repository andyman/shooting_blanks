using UnityEngine;
using System.Collections;

public class Hazard : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		int layer = other.gameObject.layer;
		
		Debug.Log ("Hazard Trigger enter " + other.name);
		
		// check the layer
		if (LayerConstants.PlayerFlag == layer)
		{
			HandlePlayerHit(other);
		}
		
	}

	void HandlePlayerHit(Collider2D other)
	{
		
		Actor otherActor = other.GetComponent<Actor>();
		
		// ignore if there is no player, or it is our own
		if (otherActor == null)
		{
			return;
		}
		
		otherActor.ApplyHit(this);
	}
}
