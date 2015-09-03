using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {

	public enum MenuType {MainMenu, EncyclopediaMenu, GodChoiceMenu, CustomizeMenu};

	public MainMenu mainMenu;
	GameControl gameControl;
	bool MainMenuIsOn = true;
	EncyclopediaMenu encyclopediaMenu;
	bool EncyclopediaMenuIsOn = false;
	GodChoiceMenu godChoiceMenu;
	bool GodChoiceMenuIsOn = false;
	CustomizeMenu customizeMenu;
	bool CustomizeMenuIsOn = false;
	bool TutorialIsOn = false;

	GUIStyleLibrary styleLibrary;

	void Start() {
		gameControl = gameObject.GetComponent<GameControl> ();
		mainMenu = gameObject.GetComponent<MainMenu> ();
		encyclopediaMenu = gameObject.GetComponent<EncyclopediaMenu> ();
		godChoiceMenu = gameObject.GetComponent<GodChoiceMenu> ();
		customizeMenu = gameObject.GetComponent<CustomizeMenu> ();

		styleLibrary = gameObject.GetComponent<GUIStyleLibrary> ();
	}

	public void TurnOnMenu (MenuType menu) {
		TurnOffMenus ();

		if (menu == MenuType.MainMenu) {
			MainMenu.CleanUpGameboard();
			MainMenu.errorText = "";
			mainMenu.enabled = true;
			MainMenuIsOn = true;
		} else if (menu == MenuType.EncyclopediaMenu) {
			encyclopediaMenu.enabled = true;
			EncyclopediaMenuIsOn = true;
		} else if (menu == MenuType.GodChoiceMenu) {
			godChoiceMenu.enabled = true;
			GodChoiceMenuIsOn = true;
			for (int i = 0; i < SaveData.UnlockedGods.Count; i++)
			{
				godChoiceMenu.GodChoiceSelection[ShopControl.AllGods.IndexOf(SaveData.UnlockedGods[i])] = true;
			}
		} else if (menu == MenuType.CustomizeMenu) {
			customizeMenu.enabled = true;
			CustomizeMenuIsOn = true;
			customizeMenu.FindCards();
		} else {
			Debug.LogError("An unknown menu type was passed.");
		}
	}

	public void TurnOnTutorial() {
		TurnOffMenus ();
		Tutorial.TutorialLevel = 1;
		gameControl.BeginGame();
	}
	
	public void TurnOffMenus() {
		mainMenu.enabled = false;
		encyclopediaMenu.enabled = false;
		godChoiceMenu.enabled = false;
		customizeMenu.enabled = false;

		EncyclopediaMenuIsOn = false;
		GodChoiceMenuIsOn = false;
		CustomizeMenuIsOn = false;
		MainMenuIsOn = false;
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
