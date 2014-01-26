using UnityEngine;
using System.Collections;

public class RandomScaleLocation : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		// set up a random location/rotation
		Vector3 randomPosition = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f), 0.0f);
		Quaternion randomRotation = Quaternion.Euler(0,0, Random.Range (0, 360.0f));
		Vector3 randomScale = transform.localScale;
		
		randomScale.x = Random.Range (0.5f, 2.0f);
		randomScale.y = Random.Range (0.5f, 2.0f);
		
		transform.localScale = randomScale;
		transform.position = randomPosition;
		transform.rotation = randomRotation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
