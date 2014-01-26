using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	public Texture[] characterPortraits;
	public string[] characterBlurb;

	public int[] playerSelection;
	public Texture2D buttonNotSelectedBackground;
	public Texture2D buttonSelectedBackground;
	public Texture2D buttonHoverBackground;

	public string[] levelNames;
	public Texture2D[] levelImages;
	public int[] levelBuildIndices;

	public static int player0 = 0;
	public static int player1 = 3;

	// Use this for initialization
	void Start () {
		playerSelection[0] = player0;
		playerSelection[1] = player1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		GUIStyle headingLabelStyle = new GUIStyle(GUI.skin.label);
		headingLabelStyle.fontSize = 18;
		headingLabelStyle.fontStyle = FontStyle.Bold;

		GUIStyle headingHelpStyle = new GUIStyle(GUI.skin.label);
		headingHelpStyle.fontSize = 12;
		headingHelpStyle.normal.textColor = new Color32(255,255,255, 128);

		GUIStyle unselectedButton = new GUIStyle(GUI.skin.button);
		unselectedButton.normal.background = buttonNotSelectedBackground;
		unselectedButton.active.background = buttonNotSelectedBackground;
		unselectedButton.hover.background = buttonHoverBackground;
		unselectedButton.border = new RectOffset();
		GUIStyle selectedButton = new GUIStyle(GUI.skin.button);
		selectedButton.normal.background = buttonSelectedBackground;
		selectedButton.active.background = buttonSelectedBackground;
		selectedButton.hover.background = buttonHoverBackground;

		selectedButton.border = new RectOffset();


		// display player 1
		GUI.BeginGroup(new Rect(600, 20, 450, 200));
		GUI.Label(new Rect(0, 0, 300, 30), "Player 1", headingLabelStyle);
		//GUI.Label(new Rect(100, 0, 300, 30), "Move: w,s,a,d  Fire: spacebar", headingHelpStyle);

		for(int i = 0; i < characterPortraits.Length; i++)
		{
			GUIStyle buttonStyle = (playerSelection[0] == i) ? selectedButton : unselectedButton;
			if (GUI.Button(new Rect(i*50, 30, 44, 44), characterPortraits[i], buttonStyle))
			{
				playerSelection[0] = i;
				player0 = i;
			}

		}
		GUI.Label(new Rect(0, 79, 300, 30), characterBlurb[playerSelection[0]], headingHelpStyle);

		GUI.EndGroup();

		// display player 2

		GUI.BeginGroup(new Rect(600, 130, 450, 200));
		GUI.Label(new Rect(0, 0, 300, 30), "Player 2", headingLabelStyle);
		//GUI.Label(new Rect(100, 0, 300, 30), "Move: arrow keys  Fire: return", headingHelpStyle);
		
		for(int i = 0; i < characterPortraits.Length; i++)
		{
			GUIStyle buttonStyle = (playerSelection[1] == i) ? selectedButton : unselectedButton;
			if (GUI.Button(new Rect(i*50, 30, 44, 44), characterPortraits[i], buttonStyle))
			{
				playerSelection[1] = i;
				player1 = i;
			}
		}

		GUI.Label(new Rect(0, 79, 300, 30), characterBlurb[playerSelection[1]], headingHelpStyle);

		GUI.EndGroup();

		// display level selector
		GUIStyle borderlessButton = new GUIStyle(GUI.skin.button);
		borderlessButton.border = new RectOffset(2,2,2,2);
		borderlessButton.padding = new RectOffset(2,2,2,2);
		borderlessButton.margin = new RectOffset(2,2,2,2);

		GUI.BeginGroup(new Rect(80,260, 960, 600));
		for(int i = 0; i < levelNames.Length; i++)
		{
			float upperOffset = ((int)(i/4))*150.0f;
			GUI.BeginGroup (new Rect((i%4)*200.0f, upperOffset, 200.0f, 200.0f));
			GUI.Label (new Rect(0,0,200.0f, 30.0f), levelNames[i]);
			if (levelImages[i] == null)
			{
				if (GUI.Button(new Rect(0,20,180,112), "Test"))
				{
					StartLevel(i);
				}
			}
			else
			{
				if (GUI.Button(new Rect(0,20,180,112), levelImages[i], borderlessButton))
				{
					StartLevel(i);
				}
			}
			GUI.EndGroup ();
		}
		GUI.EndGroup();
	}

	void StartLevel(int levelIndex)
	{
		Application.LoadLevel(levelBuildIndices[levelIndex]);
	}
}
