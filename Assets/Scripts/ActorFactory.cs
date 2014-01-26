using UnityEngine;
using System.Collections;

public class ActorFactory : MonoBehaviour {

	public Actor[] actorPrefabs;

	public static ActorFactory instance = null;

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
