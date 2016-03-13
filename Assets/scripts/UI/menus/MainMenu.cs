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

	static ClickControl S.ClickControlInst;
	static ShopControlGUI S.ShopControlGUIInst;

	public static string errorText = "";

	static ShopControl.Gods[] UnlockOrder = new ShopControl.Gods[] {ShopControl.Gods.Chac, ShopControl.Gods.Ikka, 
		ShopControl.Gods.Ixchel, ShopControl.Gods.Kinich, ShopControl.Gods.Akan, ShopControl.Gods.Ekcha};
	static int[] UnlockLevels = new int[6] {2, 4, 7, 10, 13, 16};

	void Start() {
		useGUILayout = false;
	}

    //Brings up the "you died" menu, which is here in MainMenu.
	public static void Die() {
    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().alive = false;

		MainMenuUp = false;
		DiedMenuUp = true;
		S.ClickControlInst.DisallowEveryInput ();
		S.ClickControlInst.AllowInfoInput = true;
		S.ClickControlInst.AllowInputUmbrella = true;
		MainMenu.InGame = false;

		GameObject.Find ("Dimmer").GetComponent<DimAnimate>().ForceDim();

        if (GameControl.Level >= NextUnlockLevel)
        {
            for (int i = 0; i < UnlockLevels.Length; i++)
            {
                if (GameControl.Level >= UnlockLevels[i])
                {
                    if (!SaveDataControl.UnlockedGods.Contains(UnlockOrder[i]))
                    {
                        UnlockedText += "Unlocked " + UnlockOrder[i].ToString();
                        SaveDataControl.UnlockedGods.Add(UnlockOrder[i]);
                    }
                }
            }
        }

		StateSavingControl.Save();
	}

    //This gets called when you click "OK in the "you died" menu.
    public static void CleanUpGameboard()
    {
        GameControl.Level = 0;
        SaveDataControl.Save();
        MainMenu.MainMenuUp = true;
        MainMenu.InGame = false;
        DiedMenuUp = false;
        UnlockedText = "";
        UnlockCheck();

        S.GameControlInst.DeleteAllCards();

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
			if(!SaveDataControl.UnlockedGods.Contains(UnlockOrder[i])) {
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
        if (MainMenuUp) {
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", S.GUIStyleLibraryInst.MainStyles.BlackBackground);

			//Title
            GUI.Box(new Rect(Screen.width * .1f, Screen.height * .05f, Screen.width * .8f, Screen.height * .15f), 
			        "Popul Vuh", S.GUIStyleLibraryInst.MainStyles.Title);

			//Button 1: Start new game
            
            string startGameText = StateSavingControl.StateWasSaved() ? "Resume game" : "Start new game" ;
            GUIStyle startGameStyle = StateSavingControl.StateWasSaved() ? 
                S.GUIStyleLibraryInst.MainStyles.HighlitButton : S.GUIStyleLibraryInst.MainStyles.Button;
            if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .25f, Screen.width * .6f, Screen.height * .1f), 
			               startGameText, startGameStyle)) {
                
				S.MenuControlInst.TurnOffMenus();

                // if (SaveDataControl.UnlockedGods.Count == 7) {
				// 	S.MenuControlInst.TurnOnMenu(MenuControl.MenuType.GodChoiceMenu);
				// } else 
                if (StateSavingControl.StateWasSaved()) {
					StateSavingControl.Load();
				} else {
					S.GameControlInst.BeginGame();
                }
            }

			//Button 2: Tutorial
			if(GUI.Button(new Rect(Screen.width*.2f, Screen.height*.4f, Screen.width*.6f, Screen.height*.1f), 
			              "Tutorial", S.GUIStyleLibraryInst.MainStyles.Button)) {
				S.MenuControlInst.TurnOnMenu(MenuControl.MenuType.Tutorial);
				S.GameControlInst.BeginGame();
			}
			if (!SaveDataControl.FinishedTutorial)
			{
				GUI.Box(new Rect(Screen.width * .25f, Screen.height * .475f, Screen.width * .5f, Screen.height * .05f), 
				        "Learn to play here!", S.GUIStyleLibraryInst.MainStyles.NewCardsAvailablePopup);
			}

			//Button 3: Customize Deck
			if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .55f, Screen.width * .6f, Screen.height * .1f), 
			               "Customize Deck", S.GUIStyleLibraryInst.MainStyles.Button))
            {
                SaveDataControl.NewCardsAvailable = false;
				S.MenuControlInst.TurnOnMenu(MenuControl.MenuType.CustomizeMenu);
            }
				//Subheading for Customize Deck
			if (SaveDataControl.NewCardsAvailable)
			{
				GUI.Box(new Rect(Screen.width * .25f, Screen.height * .625f, Screen.width * .5f, Screen.height * .05f), 
				        "New cards available!", S.GUIStyleLibraryInst.MainStyles.NewCardsAvailablePopup);
			}

			//Button 4: Encyclopedia
			if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .7f, Screen.width * .6f, Screen.height * .1f),
			               "Encyclopedia", S.GUIStyleLibraryInst.MainStyles.Button))
			{
				S.MenuControlInst.TurnOnMenu(MenuControl.MenuType.EncyclopediaMenu);
			}

			//Button 5: Reset data
			if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .85f, Screen.width * .6f, Screen.height * .1f), 
			               "Reset unlocked data", S.GUIStyleLibraryInst.MainStyles.Button))
			{
				DeleteDataMenuUp = true;
				MainMenuUp = false;
			}

			//Left Sidebar
			GUIContent CardsUnlocked = new GUIContent("You have unlocked \n" + SaveDataControl.UnlockedCards.Count + "/" + CardLibrary.Lib.Count + "\ncards");
            GUI.Box(new Rect(Screen.width * .025f, Screen.height * .3f, Screen.width * .15f, Screen.height * .6f), 
			        CardsUnlocked, S.GUIStyleLibraryInst.MainStyles.Sidebar);

			//Right Sidebar
            GUI.BeginGroup(new Rect(Screen.width * .825f, Screen.height * .3f, Screen.width * .15f, Screen.height * .6f));
			GUI.Box(new Rect(0, Screen.height * .0f, Screen.width * .15f, Screen.height * .2f), "Next God to unlock:", S.GUIStyleLibraryInst.MainStyles.Sidebar);
            GUI.Box(new Rect(0, Screen.height * .2f, Screen.width * .15f, Screen.height * .2f), new GUIContent("\n" + NextGodToUnlock.ToString(),
			        S.ShopControlGUIInst.GodFullTextures[ShopControl.AllGods.IndexOf(NextGodToUnlock)]), S.GUIStyleLibraryInst.MainStyles.Sidebar);
            GUI.Box(new Rect(0, Screen.height * .4f, Screen.width * .15f, Screen.height * .2f),
			        NextGodToUnlock.ToString() + " will be unlocked when you reach level " + NextUnlockLevel.ToString(), S.GUIStyleLibraryInst.MainStyles.Sidebar);
            GUI.EndGroup();

        }
        #endregion

        #region Delete Data Menu
        if (DeleteDataMenuUp)
        {
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", S.GUIStyleLibraryInst.MainStyles.BlackBackground);
			
			GUI.Box(new Rect(Screen.width * .2f, Screen.height * .1f, Screen.width * .6f, Screen.height * .4f), 
			        "Are you sure?", S.GUIStyleLibraryInst.MainStyles.NeutralButton);
            if (GUI.Button(new Rect(Screen.width * .6f, Screen.height * .8f, Screen.width * .3f, Screen.height * .15f), 
			               "Yes!", S.GUIStyleLibraryInst.MainStyles.Button))
            {
                MainMenuUp = true;
                DeleteDataMenuUp = false;
                SaveDataControl.NewSaveFile();
				SaveDataControl.Save();
				StateSavingControl.DeleteState();
            }
            if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .8f, Screen.width * .3f, Screen.height * .15f), 
			               "Back", S.GUIStyleLibraryInst.MainStyles.Button))
            {
                DeleteDataMenuUp = false;
                SaveDataControl.Save();
                MainMenuUp = true;
            }

        }
        #endregion

        #region Died Menu
        if (DiedMenuUp)
        {
			GUIStyle YouDiedStyle = new GUIStyle(S.GUIStyleLibraryInst.MainStyles.NeutralButton);
			YouDiedStyle.fontSize = 40;
            GUI.Box(new Rect(Screen.width * .1f, Screen.height * .2f, Screen.width * .8f, Screen.height * .3f), 
			        "You died!", YouDiedStyle);
            if (UnlockedText != "")
            {
                GUI.Box(new Rect(Screen.width * .1f, Screen.height * .55f, Screen.width * .8f, Screen.height * .15f), 
				        "Got to level " + GameControl.Level.ToString() + "\n" + UnlockedText, 
				        S.GUIStyleLibraryInst.MainStyles.NeutralButton);
            }
            if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .75f, Screen.width * .8f, Screen.height * .15f), 
			               "Return to main menu!", S.GUIStyleLibraryInst.MainStyles.Button))
            {
                CleanUpGameboard();
            }
        }
        #endregion
    }
}
