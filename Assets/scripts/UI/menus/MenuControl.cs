using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {

	public enum MenuType {MainMenu, EncyclopediaMenu, GodChoiceMenu, CustomizeMenu, Tutorial};

	public MainMenu S.MainMenuInst;
	bool MainMenuIsOn = true;
	bool EncyclopediaMenuIsOn = false;
	bool GodChoiceMenuIsOn = false;
	bool CustomizeMenuIsOn = false;
	bool TutorialIsOn = false;

	public void TurnOnMenu (MenuType menu) {
		TurnOffMenus ();

		if (menu == MenuType.MainMenu) {
			MainMenu.errorText = "";
			S.MainMenuInst.enabled = true;
			MainMenuIsOn = true;
		} else if (menu == MenuType.EncyclopediaMenu) {
			S.EncyclopediaMenuInst.enabled = true;
			EncyclopediaMenuIsOn = true;
		} else if (menu == MenuType.GodChoiceMenu) {
			S.GodChoiceMenuInst.enabled = true;
			GodChoiceMenuIsOn = true;
			for (int i = 0; i < SaveDataControl.UnlockedGods.Count; i++) {
				S.GodChoiceMenuInst.GodChoiceSelection [ShopControl.AllGods.IndexOf (SaveDataControl.UnlockedGods [i])] = true;
			}
		} else if (menu == MenuType.CustomizeMenu) {
			S.CustomizeMenuInst.enabled = true;
			CustomizeMenuIsOn = true;
			S.CustomizeMenuInst.FindCards ();
		} else if (menu == MenuType.Tutorial) {
			S.TutorialInst.enabled = true;
			Tutorial.TutorialLevel = 1;
			TutorialIsOn = true;
		} else {
			Debug.LogError("An unknown menu type was passed.");
		}
	}
	
	public void TurnOffMenus() {
		S.MainMenuInst.enabled = false;
		S.EncyclopediaMenuInst.enabled = false;
		S.GodChoiceMenuInst.enabled = false;
		S.CustomizeMenuInst.enabled = false;
		S.TutorialInst.enabled = false;

		EncyclopediaMenuIsOn = false;
		GodChoiceMenuIsOn = false;
		CustomizeMenuIsOn = false;
		MainMenuIsOn = false;
		TutorialIsOn = false;
	}

	public void Die() {
		TurnOnMenu (MenuType.MainMenu);
		MainMenu.Die ();
	}

	public bool AnyMenuIsUp() {
		return EncyclopediaMenuIsOn |
			GodChoiceMenuIsOn |
			CustomizeMenuIsOn |
				MainMenuIsOn;
	}
}
