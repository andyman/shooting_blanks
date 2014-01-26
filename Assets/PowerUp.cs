﻿using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	float powerUpReappearDelay = 2.0f;
	float messageDisplayTime = 2.0f;

	void OnTriggerEnter2D(Collider2D other) {
		int layer = other.gameObject.layer;
		
		Debug.Log ("Power Up Trigger enter " + other.name);
		
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
		
		otherActor.ApplyPowerUp(this);
		
		GUIText guiText = LevelController.instance.genericMessage;

		guiText.text = "Switched opponent's real actor!";
		guiText.gameObject.SetActive (true);

		gameObject.SetActive(false);
		Invoke ("Reappear", powerUpReappearDelay);
		Invoke ("MessageOff", messageDisplayTime);

	}
	

	void MessageOff()
	{
		GUIText guiText = LevelController.instance.genericMessage;
		guiText.gameObject.SetActive (false);
	}

	void Reappear()
	{
		Vector3 randomPosition = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f), 0.0f);
		Quaternion randomRotation = Quaternion.Euler(0,0, Random.Range (0, 360.0f));
		
		transform.position = randomPosition;
		transform.rotation = randomRotation;
		gameObject.SetActive(true);
	}


}
