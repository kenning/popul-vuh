using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public static bool InGame;

	public static bool MainMenuUp = true;
	public static bool DeleteDataMenuUp = false;
    public static bool DiedMenuUp = false;

    static ShopControl.Gods NextGodToUnlock = ShopControl.Gods.Buluc;
	static int NextUnlockLevel = 0;
	static string UnlockedText = "";

	MenuControl menuControl;
	static ClickControl clickControl;
	GameControl gameControl;
	GUIStyleLibrary styleLibrary;
	static ShopControlGUI shopControlGUI;

	public static string errorText = "";

	static ShopControl.Gods[] UnlockOrder = new ShopControl.Gods[] {ShopControl.Gods.Chac, ShopControl.Gods.Ikka, 
		ShopControl.Gods.Ixchel, ShopControl.Gods.Kinich, ShopControl.Gods.Akan, ShopControl.Gods.Ekcha};
	static int[] UnlockLevels = new int[6] {2, 4, 6, 8, 10, 12};

	void Start() {
		useGUILayout = false;
		menuControl = gameObject.GetComponent<MenuControl> ();
		gameControl = gameObject.GetComponent<GameControl> ();
		clickControl = gameObject.GetComponent<ClickControl> ();
		shopControlGUI = gameObject.GetComponent<ShopControlGUI> ();
		styleLibrary = gameObject.GetComponent<GUIStyleLibrary> ();
	}

    //Brings up the "you died" menu, which is here in MainMenu.
	public static void Die() 
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().alive = false;

		MainMenuUp = false;
		DiedMenuUp = true;
		clickControl.DisallowEveryInput ();
		clickControl.AllowInfoInput = true;
		clickControl.AllowInputUmbrella = true;
		MainMenu.InGame = false;

		GameObject.Find ("Dimmer").GetComponent<DimAnimate>().ForceDim();

//        GameControl gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
//        gameControl.TransformPlayer(false);

        if (GameControl.Level >= NextUnlockLevel)
        {
            for (int i = 0; i < UnlockLevels.Length; i++)
            {
                if (GameControl.Level >= UnlockLevels[i])
                {
                    if (!SaveData.UnlockedGods.Contains(UnlockOrder[i]))
                    {
                        UnlockedText += "Unlocked " + UnlockOrder[i].ToString();
                        SaveData.UnlockedGods.Add(UnlockOrder[i]);
                    }
                }
            }
        }
	}

    //This gets called when you click "OK in the "you died" menu.
    public static void CleanUpGameboard()
    {
        GameControl.Level = 0;
        SaveLoad.Save();
        MainMenu.MainMenuUp = true;
        MainMenu.InGame = false;
        DiedMenuUp = false;
        shopControlGUI.TurnOffShopControlGUIs();
        UnlockedText = "";
        UnlockCheck();

        GameControl gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        gameControl.DeleteAllCards();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject GO in enemies)
        {
            Destroy(GO);
        }

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("obstacle");
        foreach (GameObject GO in obstacles)
        {
            Destroy(GO);
        }
    }

    //Sets NextGodToUnlock and NextUnlockLevel, which are here in MainMenu. 
    //This method gets called when you click "Return to main menu" in the You Died menu.
    //Those variables show up on the main menu and also get checked in MainMenu.Die() to see if you unlocked something new.
	public static void UnlockCheck() {
		for(int i = 0; i < UnlockOrder.Length; i++) {
			if(!SaveData.UnlockedGods.Contains(UnlockOrder[i])) {
				NextGodToUnlock = UnlockOrder[i];
				NextUnlockLevel = UnlockLevels[i];
				break;
			}
		}
	}
    //Main menu, Delete data menu and You died menu.
    void OnGUI()
    {
		GUI.depth = 1;

		GUI.depth = 0;

        #region Main Menu
        if (MainMenuUp)
        {
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", styleLibrary.MainStyles.BlackBackground);

			//Title
            GUI.Box(new Rect(Screen.width * .1f, Screen.height * .05f, Screen.width * .8f, Screen.height * .15f), 
			        "Popul Vuh", styleLibrary.MainStyles.Title);

			//Button 1: Start new game
            if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .25f, Screen.width * .6f, Screen.height * .1f), 
			               "Start new game", styleLibrary.MainStyles.Button))
            {
                
				menuControl.TurnOffMenus();

                if (SaveData.UnlockedGods.Count == 7)
                {
					menuControl.TurnOnMenu(MenuControl.MenuType.GodChoiceMenu);
                }
                else
                {
					gameControl.BeginGame();
                }
            }

			//Button 2: Tutorial
			if(GUI.Button(new Rect(Screen.width*.2f, Screen.height*.4f, Screen.width*.6f, Screen.height*.1f), 
			              "Tutorial", styleLibrary.MainStyles.Button)) {
				menuControl.TurnOnMenu(MenuControl.MenuType.Tutorial);
			}
			if (!SaveData.FinishedTutorial)
			{
				GUI.Box(new Rect(Screen.width * .25f, Screen.height * .475f, Screen.width * .5f, Screen.height * .05f), 
				        "Learn to play here!", styleLibrary.MainStyles.NewCardsAvailablePopup);
			}

			//Button 3: Customize Deck
			if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .55f, Screen.width * .6f, Screen.height * .1f), 
			               "Customize Deck", styleLibrary.MainStyles.Button))
            {
                SaveData.NewCardsAvailable = false;
				menuControl.TurnOnMenu(MenuControl.MenuType.CustomizeMenu);
            }
				//Subheading for Customize Deck
			if (SaveData.NewCardsAvailable)
			{
				GUI.Box(new Rect(Screen.width * .25f, Screen.height * .625f, Screen.width * .5f, Screen.height * .05f), 
				        "New cards available!", styleLibrary.MainStyles.NewCardsAvailablePopup);
			}

			//Button 4: Encyclopedia
			if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .7f, Screen.width * .6f, Screen.height * .1f),
			               "Encyclopedia", styleLibrary.MainStyles.Button))
			{
				menuControl.TurnOnMenu(MenuControl.MenuType.EncyclopediaMenu);
			}

			//Button 5: Reset data
			if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .85f, Screen.width * .6f, Screen.height * .1f), 
			               "Reset unlocked data", styleLibrary.MainStyles.Button))
			{
				DeleteDataMenuUp = true;
				MainMenuUp = false;
			}

			//Left Sidebar
			GUIContent CardsUnlocked = new GUIContent("You have unlocked \n" + SaveData.UnlockedCards.Count + "/" + CardLibrary.Lib.Count + "\ncards");
            GUI.Box(new Rect(Screen.width * .025f, Screen.height * .3f, Screen.width * .15f, Screen.height * .6f), 
			        CardsUnlocked, styleLibrary.MainStyles.Sidebar);

			//Right Sidebar
            GUI.BeginGroup(new Rect(Screen.width * .825f, Screen.height * .3f, Screen.width * .15f, Screen.height * .6f));
			GUI.Box(new Rect(0, Screen.height * .0f, Screen.width * .15f, Screen.height * .2f), "Next God to unlock:", styleLibrary.MainStyles.Sidebar);
            GUI.Box(new Rect(0, Screen.height * .2f, Screen.width * .15f, Screen.height * .2f), new GUIContent("\n" + NextGodToUnlock.ToString(),
			        shopControlGUI.GodFullTextures[ShopControl.AllGods.IndexOf(NextGodToUnlock)]), styleLibrary.MainStyles.Sidebar);
            GUI.Box(new Rect(0, Screen.height * .4f, Screen.width * .15f, Screen.height * .2f),
			        NextGodToUnlock.ToString() + " will be unlocked when you reach level " + NextUnlockLevel.ToString(), styleLibrary.MainStyles.Sidebar);
            GUI.EndGroup();

        }
        #endregion

        #region Delete Data Menu
        if (DeleteDataMenuUp)
        {
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", styleLibrary.MainStyles.BlackBackground);
			
			GUI.Box(new Rect(Screen.width * .2f, Screen.height * .1f, Screen.width * .6f, Screen.height * .4f), 
			        "Are you sure?", styleLibrary.MainStyles.NeutralButton);
            if (GUI.Button(new Rect(Screen.width * .6f, Screen.height * .8f, Screen.width * .3f, Screen.height * .15f), 
			               "Yes!", styleLibrary.MainStyles.Button))
            {
                MainMenuUp = true;
                DeleteDataMenuUp = false;
                SaveData.NewSaveFile();
				SaveLoad.Save();
            }
            if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .8f, Screen.width * .3f, Screen.height * .15f), 
			               "Back", styleLibrary.MainStyles.Button))
            {
                DeleteDataMenuUp = false;
                SaveLoad.Save();
                MainMenuUp = true;
            }

        }
        #endregion

        #region Died Menu
        if (DiedMenuUp)
        {
			GUIStyle YouDiedStyle = new GUIStyle(styleLibrary.MainStyles.NeutralButton);
			YouDiedStyle.fontSize = 40;
            GUI.Box(new Rect(Screen.width * .1f, Screen.height * .2f, Screen.width * .8f, Screen.height * .3f), 
			        "You died!", YouDiedStyle);
            if (UnlockedText != "")
            {
                GUI.Box(new Rect(Screen.width * .1f, Screen.height * .55f, Screen.width * .8f, Screen.height * .15f), 
				        "Got to level " + GameControl.Level.ToString() + "\n" + UnlockedText, 
				        styleLibrary.MainStyles.NeutralButton);
            }
            if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .75f, Screen.width * .8f, Screen.height * .15f), 
			               "Return to main menu!", styleLibrary.MainStyles.Button))
            {
                CleanUpGameboard();
            }
        }
        #endregion
    }
}
