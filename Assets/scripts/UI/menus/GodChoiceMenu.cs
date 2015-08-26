using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GodChoiceMenu : MonoBehaviour {

	// There is something bugged in this menu, and it makes the game unplayable if you have unlocked all gods.

	public static bool GodChoiceMenuUp = false;

	public bool[] GodChoiceSelection = new bool[7];
	GameControl gameControl;
	ShopControl shopControl;
	ShopControlGUI shopControlGUI;

	GUIStyleLibrary styleLibrary;

	void Start() {
		useGUILayout = false;
		gameControl = gameObject.GetComponent<GameControl> ();
		shopControl = gameObject.GetComponent<ShopControl> ();
		shopControlGUI = gameObject.GetComponent<ShopControlGUI> ();
		styleLibrary = gameObject.GetComponent<GUIStyleLibrary> ();
		GodChoiceSelection = new bool[] {false, false, false, false, false, false, false};
	}

	void startGameOrGoToNextLevel() {
		if (!MainMenu.InGame) {
			gameControl.BeginGame ();
		} else {
			gameControl.StartNewLevel ();
		}
	}

	void OnGUI () {

		GUI.depth = 0;

		if(GodChoiceMenuUp) {
			for(int i = 0; i < SaveData.UnlockedGods.Count; i++) {
				if(shopControl == null) shopControl = gameObject.GetComponent<ShopControl>();
				int thisGodNumber = ShopControl.AllGods.IndexOf(SaveData.UnlockedGods[i]);
				GodChoiceSelection[thisGodNumber] = 
					GUI.Toggle(new Rect(Screen.width*.1f, Screen.height*.1f*i, Screen.width*.6f, Screen.height*.1f), 
					           GodChoiceSelection[thisGodNumber], SaveData.UnlockedGods[i].ToString(), styleLibrary.GodChoiceStyles.GodChoiceToggle);
				GUI.Box(new Rect(Screen.width*.15f, Screen.height*.1f*i + Screen.height*.06f, Screen.width*.6f, Screen.height*.030f), 
				        ShopControl.GodDescriptions[thisGodNumber], styleLibrary.GodChoiceStyles.GodChoiceToggleText);
				if(GodChoiceSelection[ShopControl.AllGods.IndexOf(SaveData.UnlockedGods[i])]) {
					GUI.Box(new Rect(Screen.width*.75f, Screen.height*.1f*i, Screen.width*.2f, Screen.height*.1f), 
					        shopControlGUI.GodIcons[thisGodNumber]);
				}
			}

			if(!MainMenu.InGame) {
				if(GUI.Button(new Rect(Screen.width*.1f, Screen.height*.8f, Screen.width*.3f, Screen.height*.15f), 
				              "Back", styleLibrary.GodChoiceStyles.BackButton)) {
					GodChoiceMenuUp = false;
					SaveLoad.Save();
					MainMenu.MainMenuUp = true;
				}
			}
			if(GUI.Button(new Rect(Screen.width*.6f, Screen.height*.8f, Screen.width*.3f, Screen.height*.15f), 
			              "Start", styleLibrary.GodChoiceStyles.Title)) {
				if(GoalLibrary.NumberOfGoalsPossible(GodChoiceSelection) > 3) {
					GodChoiceMenuUp = false;
					startGameOrGoToNextLevel();
				}
				else {
					MainMenu.errorText = "You must select more gods than\nthat to have enough goals to play.";
				}
			}
			if(MainMenu.errorText != "") {
				GUI.Box(new Rect(Screen.width*.2f, Screen.height*.7f, Screen.width*.6f, Screen.height*.075f), MainMenu.errorText);
			}
		}
	}
}
