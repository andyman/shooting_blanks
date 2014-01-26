using UnityEngine;
using System.Collections;

/** this class has the level settings, and sets up the players */
public class LevelController : MonoBehaviour {
	
	public static LevelController instance = null;
	public int[] actorCounts; // how many clones for each player

	public GUIText winnerMessage; // reference to the winner message UI
	public GUIText genericMessage;

	// array of holders, with each holder containing the actors for a player.
	// leave empty in the inspector unless you are preplacing actors
	public Transform[] playerHolders; 

	int winner = -1; // index of the player that won. -1 for none yet.
	int players = 2; // number of players
	public Actor[] actorPrefabs = {null, null}; // the prefab for each player

	void Awake() {
		LevelController.instance = this;
		// set the actor prefabs based on what was in the menu


	}

	// Use this for initialization
	void Start () {

		actorPrefabs[0] = ActorFactory.instance.actorPrefabs[MenuController.player0];
		actorPrefabs[1] = ActorFactory.instance.actorPrefabs[MenuController.player1];
		InitializePlayers();
	}


	// Update is called once per frame
	void Update () {
	
	}

	public void InitializePlayers()
	{
		// create the array of player holders if there is not already one
		if (playerHolders == null || playerHolders.Length != players) 
		{
			playerHolders = new Transform[players];
		}
		
		for(int player = 0; player < players; player++)
		{
			// create an empty holder gameobject for each player for easier tracking later, if it is not already created
			Transform playerHolder = playerHolders[player];
			if (playerHolder == null)
			{
				playerHolder = (new GameObject("PlayerWrapper" + player)).transform;
				playerHolders[player] = playerHolder;
			}
			
			// pick one at random to be the real actor
			int realActorIndex = Random.Range (0, actorCounts[player]);
			
			// create however many number of actors
			for(int i = 0; i < actorCounts[player]; i++)
			{
				CreateActor(player, realActorIndex == i, playerHolder);
			}
		}
	}
	// creates an actor for a player, at a random location, as a child of the playerHolder
	public void CreateActor(int player, bool isReal, Transform playerHolder)
	{

		// set up a random location/rotation
		Vector3 randomPosition = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f), 0.0f);
		Quaternion randomRotation = Quaternion.Euler(0,0, Random.Range (0, 360.0f));
		
		Actor actor = Instantiate(actorPrefabs[player], randomPosition, randomRotation) as Actor;

		// name the actor for easier reference in the editor
		actor.name = actor.name + " p" + player;

		// assign it to a player and whether it is real or not
		actor.player = player;
		actor.real = isReal;
		
		// put the new actor inside the holder for that player for easy tracking later
		actor.transform.parent = playerHolder.transform;
	}

	// called whenever an actor is killed
	public void ReportActorDeath(Actor actor)
	{
		// see whose actor died
		int player = actor.player;

		// first remove from parent holder
		actor.transform.parent = null;

		// see how many are left for that player
		int remainingActorsCount = CountPlayerActors(player);

		// handle player elimination
		if (remainingActorsCount == 0)
		{
			CheckForWinner();
		}
		// otherwise see if we need to assign another real clone
		else if (actor.real && CountPlayerRealActors(player) == 0)
		{
			AssignRandomRealActor(player);
		}
		
	}

	// makes one of the actors real
	public void AssignRandomRealActor(int player)
	{
		int nextRealActorIndex = Random.Range(0, CountPlayerActors(player));
		Actor[] actors = GetActorsForPlayer(player);
		actors[nextRealActorIndex].real = true; // this just became real for that clone
	}

	// count the number of actors left for the player
	public int CountPlayerActors(int player)
	{
		return playerHolders[player].childCount;
	}

	// get the array of remaining actors for a player
	public Actor[] GetActorsForPlayer(int player)
	{
		return playerHolders[player].GetComponentsInChildren<Actor>();

	}

	// see if there is one player remaining with actors
	public void CheckForWinner()
	{
		int playersWithActors = 0;
		int lastPlayerWithActor = -1;

		for(int player = 0; player < players; player++)
		{
			if (CountPlayerActors(player) > 0)
			{
				playersWithActors++;
				lastPlayerWithActor = player;
			}
		}

		if (playersWithActors == 1)
		{
			winner = lastPlayerWithActor;
			ShowWinnerMessage(winner);
			DisableAllActors();
		}
	}

	// shows the winner message for a player
	public void ShowWinnerMessage(int player)
	{
		if (winnerMessage == null)
		{
			Debug.LogWarning ("No winner gui assigned to LevelController");
			return;
		}

		winnerMessage.text = "Player " + (player+1) + " Wins!";
		winnerMessage.gameObject.SetActive(true);
	}

	// disables movement for all actors
	public void DisableAllActors()
	{
		for(int player = 0; player < players; player++)
		{
			SetActorsEnabledForPlayer(player, false);
		}
	}

	public void SetActorsEnabledForPlayer(int player, bool shouldEnable)
	{
		Actor[] actors = GetActorsForPlayer(player);
		foreach(Actor actor in actors)
		{
			actor.enabled = shouldEnable;
			actor.rigidbody2D.isKinematic = shouldEnable;
			actor.rigidbody2D.velocity = Vector2.zero;
		}
	}


	// count the number of real actors for a player
	public int CountPlayerRealActors(int player)
	{
		Actor[] actors = GetActorsForPlayer(player);
		int count = 0;
		foreach(Actor actor in actors)
		{
			if (actor.real)
			{
				count++;
			}
		}

		return count;
	}

	void OnGUI()
	{
		// displaying the play again button
		if (winner != -1)
		{
			if (GUI.Button (new Rect(Screen.width/2 - 125, Screen.height - 100, 250, 44), "Play Again"))
			{
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}

	// return the index of the real actor for the player
	int FindRealActor(int player)
	{
		Actor[] actors = GetActorsForPlayer(player);
		for(int actor = 0; actor < actors.Length; actor++) {
			if(actors[actor].real) return actor;
		}
		Debug.Log ("FindRealActor could not find a real actor for player # " + player);
		return -1;
	}


	// Swap real actor for another player
	public void ApplyPowerUp(Actor actor)
	{
		int player = actor.player;
		int otherPlayer = (player + 1) % players;
		int otherPlayerRealActor = FindRealActor(otherPlayer);
		int otherPlayerActorCount = CountPlayerActors(otherPlayer);

		if( otherPlayerActorCount == 0) { return; } // power up does nothing if only one actor left
		Actor[] actors = GetActorsForPlayer(otherPlayer);
		actors[otherPlayerRealActor].real = false;
		actors [(otherPlayerRealActor + 1) % otherPlayerActorCount].real = true;
	}
}
