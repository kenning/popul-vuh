using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GodChoiceMenu : MonoBehaviour {

	// There is something bugged in this menu, and it makes the game unplayable if you have unlocked all gods.

	public bool[] GodChoiceSelection = new bool[7];
	ShopControl shopControl;
	ShopControlGUI shopControlGUI;
	MenuControl menuControl;

	GUIStyleLibrary styleLibrary;

	void Start() {
		useGUILayout = false;
		shopControl = gameObject.GetComponent<ShopControl> ();
		shopControlGUI = gameObject.GetComponent<ShopControlGUI> ();
		styleLibrary = gameObject.GetComponent<GUIStyleLibrary> ();
		menuControl = gameObject.GetComponent<MenuControl> ();
		GodChoiceSelection = new bool[] {false, false, false, false, false, false, false};
	}

	void startGameOrGoToNextLevel() {
		if (!MainMenu.InGame) {
			S.GameControlInst.BeginGame ();
		} else {
			S.GameControlInst.StartNewLevel ();
		}
	}

	void OnGUI () {

		GUI.depth = 1;
		
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", styleLibrary.MainStyles.BlackBackground);
		
		GUI.depth = 0;

		for(int i = 0; i < SaveDataControl.UnlockedGods.Count; i++) {
			if(shopControl == null) shopControl = gameObject.GetComponent<ShopControl>();
			int thisGodNumber = ShopControl.AllGods.IndexOf(SaveDataControl.UnlockedGods[i]);
			GodChoiceSelection[thisGodNumber] = 
				GUI.Toggle(new Rect(Screen.width*.1f, Screen.height*.1f*i, Screen.width*.6f, Screen.height*.1f), 
				           GodChoiceSelection[thisGodNumber], SaveDataControl.UnlockedGods[i].ToString(), styleLibrary.GodChoiceStyles.GodChoiceToggle);
			GUI.Box(new Rect(Screen.width*.15f, Screen.height*.1f*i + Screen.height*.06f, Screen.width*.6f, Screen.height*.030f), 
			        ShopControl.GodDescriptions[thisGodNumber], styleLibrary.GodChoiceStyles.GodChoiceToggleText);
			if(GodChoiceSelection[ShopControl.AllGods.IndexOf(SaveDataControl.UnlockedGods[i])]) {
				GUI.Box(new Rect(Screen.width*.75f, Screen.height*.1f*i, Screen.width*.2f, Screen.height*.1f), 
				        shopControlGUI.GodIcons[thisGodNumber]);
			}
		}

		if(!MainMenu.InGame) {
			if(GUI.Button(new Rect(Screen.width*.1f, Screen.height*.8f, Screen.width*.3f, Screen.height*.15f), 
			              "Back", styleLibrary.GodChoiceStyles.BackButton)) {
				menuControl.TurnOnMenu(MenuControl.MenuType.MainMenu);
				SaveDataControl.Save();
			}
		}
		if(GUI.Button(new Rect(Screen.width*.6f, Screen.height*.8f, Screen.width*.3f, Screen.height*.15f), 
		              "Start", styleLibrary.GodChoiceStyles.Title)) {
			if(GoalLibrary.NumberOfGoalsPossible(GodChoiceSelection) > 3) {
				menuControl.TurnOffMenus();
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
