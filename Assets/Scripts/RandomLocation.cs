using UnityEngine;
using System.Collections;

public class RandomLocation : MonoBehaviour {
	
	float collisionSphereRadius = 5.0f;

	// Use this for initialization
	void Start () {
		// set up a random location/rotation		
		Vector3 randomPosition = new Vector3(0.0f, 0.0f, 0.0f);
		do {
			randomPosition.x = Random.Range(-10.0f, 10.0f);
			randomPosition.y = Random.Range(-5.0f, 5.0f);
		} while (Physics.CheckSphere(randomPosition, collisionSphereRadius));
		Quaternion randomRotation = Quaternion.Euler(0,0, Random.Range (0, 360.0f));

		transform.position = randomPosition;
		transform.rotation = randomRotation;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
