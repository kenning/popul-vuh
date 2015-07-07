using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GodChoiceMenu : MonoBehaviour {

	public static bool GodChoiceMenuUp = false;

	public bool[] GodChoiceSelection = new bool[7];
//	public enum Gods {Akan, Buluc, Ekcha, Ikka, Ixchel, Kinich, Chac, none};
	ShopControl shopBoss;
//	GameControl battleBoss;

	public GUISkin GODCHOICEMENUGUISKIN;

	void Start() {
//		battleBoss = gameObject.GetComponent<GameControl> ();
		shopBoss = gameObject.GetComponent<ShopControl> ();
		GodChoiceSelection = new bool[] {false, false, false, false, false, false, false};
	}

	void OnGUI () {

		GUI.depth = 0;

		if(GodChoiceMenuUp) {
			for(int i = 0; i < SaveData.UnlockedGods.Count; i++) {
				if(shopBoss == null) shopBoss = gameObject.GetComponent<ShopControl>();
				int thisGodNumber = ShopControl.AllGods.IndexOf(SaveData.UnlockedGods[i]);
				GodChoiceSelection[thisGodNumber] = 
					GUI.Toggle(new Rect(Screen.width*.1f, Screen.height*.1f*i, Screen.width*.6f, Screen.height*.1f), 
					           GodChoiceSelection[thisGodNumber], SaveData.UnlockedGods[i].ToString(), GODCHOICEMENUGUISKIN.toggle);
				GUI.Box(new Rect(Screen.width*.15f, Screen.height*.1f*i + Screen.height*.06f, Screen.width*.6f, Screen.height*.030f), ShopControl.GodDescriptions[thisGodNumber], GODCHOICEMENUGUISKIN.textField);
				if(GodChoiceSelection[ShopControl.AllGods.IndexOf(SaveData.UnlockedGods[i])]) {
					GUI.Box(new Rect(Screen.width*.75f, Screen.height*.1f*i, Screen.width*.2f, Screen.height*.1f), shopBoss.GodIcons[thisGodNumber]);
				}
			}

			if(!MainMenu.InGame) {
				if(GUI.Button(new Rect(Screen.width*.1f, Screen.height*.8f, Screen.width*.3f, Screen.height*.15f), "Back", GODCHOICEMENUGUISKIN.button)) {
					GodChoiceMenuUp = false;
					SaveLoad.Save();
					MainMenu.MainMenuUp = true;
				}
			}
			if(GUI.Button(new Rect(Screen.width*.6f, Screen.height*.8f, Screen.width*.3f, Screen.height*.15f), "Start", GODCHOICEMENUGUISKIN.customStyles[0])) {
				if(GoalLibrary.NumberOfGoalsPossible(GodChoiceSelection) > 3) {
					GodChoiceMenuUp = false;
					gameObject.GetComponent<MainMenu>().BeginButton();
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
