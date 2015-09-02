using UnityEngine;
using System.Collections;

public class MenuControl : MonoBehaviour {

	public enum MenuType {MainMenu, EncyclopediaMenu, GodChoiceMenu, CustomizeMenu};
	public MainMenu mainMenu;
	public EncyclopediaMenu encyclopediaMenu;
	public GodChoiceMenu godChoiceMenu;
	public CustomizeMenu customizeMenu;

	void Start() {
		mainMenu = gameObject.GetComponent<MainMenu> ();
		encyclopediaMenu = gameObject.GetComponent<EncyclopediaMenu> ();
		godChoiceMenu = gameObject.GetComponent<GodChoiceMenu> ();
		customizeMenu = gameObject.GetComponent<CustomizeMenu> ();
	}

	public void TurnOnMenu (MenuType menu, bool turnOn) {
		TurnOffMenus ();

		if (menu == MenuType.MainMenu) {
			mainMenu.enabled = true;
		} else if (menu == MenuType.EncyclopediaMenu) {

		} else if (menu == MenuType.GodChoiceMenu) {

		} else if (menu == MenuType.CustomizeMenu) {

		} else {
			Debug.LogError("An unknown menu type was passed.");
		}
	}

	public void TurnOffMenus() {
		mainMenu.enabled = false;
		encyclopediaMenu.enabled = false;
		godChoiceMenu.enabled = false;
		customizeMenu.enabled = false;
	}

	public void Die() {
		TurnOnMenu (MenuType.MainMenu, true);
		MainMenu.Die ();
	}
}
