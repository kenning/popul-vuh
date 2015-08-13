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
    static string UnlockedText;

	static ClickControl clickControl;
	GameControl gameControl;
	ShopControl shopControl;
	GUIStyleLibrary styleLibrary;
	static ShopControlGUI shopControlGUI;

	public static string errorText = "";

	static ShopControl.Gods[] UnlockOrder = new ShopControl.Gods[] {ShopControl.Gods.Chac, ShopControl.Gods.Ikka, 
		ShopControl.Gods.Ixchel, ShopControl.Gods.Kinich, ShopControl.Gods.Akan, ShopControl.Gods.Ekcha};
	static int[] UnlockLevels = new int[6] {2, 4, 6, 8, 10, 12};
	EncyclopediaMenu encyclopediaMenu;

	void Start() {
		gameControl = gameObject.GetComponent<GameControl> ();
		clickControl = gameObject.GetComponent<ClickControl> ();
		shopControl = gameObject.GetComponent<ShopControl> ();
		shopControlGUI = gameObject.GetComponent<ShopControlGUI> ();
		encyclopediaMenu = gameObject.GetComponent<EncyclopediaMenu> ();
		styleLibrary = gameObject.GetComponent<GUIStyleLibrary> ();
	}

    //Could be called by MainMenu or by GodChoiceMenu, or by DeckAnimate. starts the game or goes to the next level.
    public void BeginButton()
    {
		MainMenu.errorText = "";

		if(!InGame) {
			gameControl.BeginGame();
			InGame = true;
		} else {
			gameControl.StartNewLevel();
		}
	}

    //Brings up the "you died" menu, which is here in MainMenu.
	public static void Die() 
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().alive = false;

        Debug.Log("Died!");

		DiedMenuUp = true;
		clickControl.DisallowEveryInput ();
		clickControl.AllowInfoInput = true;
		clickControl.AllowInputUmbrella = true;
		MainMenu.InGame = false;

        GameControl gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        gameControl.TransformPlayer(false);

        if (GameControl.Level >= NextUnlockLevel)
        {
            for (int i = 0; i < UnlockLevels.Length; i++)
            {
                if (GameControl.Level >= UnlockLevels[i])
                {
                    if (!SaveData.UnlockedGods.Contains(UnlockOrder[i]))
                    {
                        Debug.Log("hi");
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

        if (MainMenuUp | GodChoiceMenu.GodChoiceMenuUp | DeleteDataMenuUp | encyclopediaMenu.EncyclopediaMenuUp | CustomizeMenu.CustomizeMenuUp)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", styleLibrary.MainStyles.BlackBackground);
            //string text = "Moves are not plays";
            //if (GameControl.MovesArePlays) 
            //    text = "Moves are plays";
            //GameControl.MovesArePlays = GUI.Toggle(new Rect(0, 0, Screen.width * .4f, Screen.height * .1f), GameControl.MovesArePlays, text);
        }

        #region Main Menu
        if (MainMenuUp)
        {

            GUI.Box(new Rect(Screen.width * .1f, Screen.height * .05f, Screen.width * .8f, Screen.height * .15f), 
			        "Popul Vuh", styleLibrary.MainStyles.Title);

            if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .25f, Screen.width * .6f, Screen.height * .1f), 
			               "Start new game", styleLibrary.MainStyles.Button))
            {
                MainMenuUp = false;
                Tutorial.TutorialLevel = 0;

                if (SaveData.UnlockedGods.Count == 7)
                {
                    GodChoiceMenu.GodChoiceMenuUp = true;
                    GodChoiceMenu choiceMenu = gameObject.GetComponent<GodChoiceMenu>();
                    for (int i = 0; i < SaveData.UnlockedGods.Count; i++)
                    {
                        choiceMenu.GodChoiceSelection[ShopControl.AllGods.IndexOf(SaveData.UnlockedGods[i])] = true;
                    }
                }
                else
                {
                    GodChoiceMenu.GodChoiceMenuUp = false;
                    MainMenu.errorText = "";

                    BeginButton();
                }
            }

            //Unlock cards and gods
            //		if(GUI.Button(new Rect(Screen.width*.2f, Screen.height*.4f, Screen.width*.6f, Screen.height*.1f), 
			//		  "Unlock cards and gods", styleLibrary.MainStyles.Button)) {
            //			UnlockMenu tempmenu = gameControl.gameObject.GetComponent<UnlockMenu>();
            //			tempmenu.ShowMenu();
            //			MainMenuUp = false;
            //		}

            if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .55f, Screen.width * .6f, Screen.height * .1f), 
			               "Reset unlocked data", styleLibrary.MainStyles.Button))
            {
                DeleteDataMenuUp = true;
                MainMenuUp = false;
            }

            if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .7f, Screen.width * .6f, Screen.height * .1f), 
			               "Customize Starting Deck", styleLibrary.MainStyles.Button))
            {
                CustomizeMenu tempMenu = gameObject.GetComponent<CustomizeMenu>();
                tempMenu.OpenMenu();
                MainMenuUp = false;
                SaveData.NewCardsAvailable = false;
            }

			if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .85f, Screen.width * .6f, Screen.height * .1f),
			               "Encyclopedia", styleLibrary.MainStyles.Button))
			{
				EncyclopediaMenu tempMenu = gameControl.gameObject.GetComponent<EncyclopediaMenu>();
				tempMenu.ShowMenu();
				MainMenuUp = false;
			}

            if (SaveData.NewCardsAvailable)
            {
                GUI.Box(new Rect(Screen.width * .25f, Screen.height * .775f, Screen.width * .5f, Screen.height * .05f), 
				        "New cards available!", styleLibrary.MainStyles.Sidebar);
            }

            //Tutorial
            if (GUI.Button(new Rect(Screen.width * .025f, Screen.height * .3f, Screen.width * .15f, Screen.height * .6f), 
			               "Tutorial", styleLibrary.MainStyles.Sidebar))
            {
                MainMenuUp = false;
                Tutorial.TutorialLevel = 1;
                BeginButton();
            }

            GUI.BeginGroup(new Rect(Screen.width * .825f, Screen.height * .3f, Screen.width * .15f, Screen.height * .6f));
			GUI.Box(new Rect(0, Screen.height * .0f, Screen.width * .15f, Screen.height * .2f), "Next God to unlock:", styleLibrary.MainStyles.Sidebar);
            GUI.Box(new Rect(0, Screen.height * .2f, Screen.width * .15f, Screen.height * .2f), new GUIContent("\n" + NextGodToUnlock.ToString(),
			        shopControlGUI.GodFullTextures[ShopControl.AllGods.IndexOf(NextGodToUnlock)]), styleLibrary.MainStyles.Sidebar);
            GUI.Box(new Rect(0, Screen.height * .4f, Screen.width * .15f, Screen.height * .2f),
			        "Unlocked at level " + NextUnlockLevel.ToString(), styleLibrary.MainStyles.Sidebar);
            GUI.EndGroup();

        }
        #endregion

        #region Delete Data Menu
        if (DeleteDataMenuUp)
        {
            GUI.Box(new Rect(Screen.width * .2f, Screen.height * .1f, Screen.width * .6f, Screen.height * .4f), 
			        "Are you sure?", styleLibrary.MainStyles.Title);
            if (GUI.Button(new Rect(Screen.width * .6f, Screen.height * .8f, Screen.width * .3f, Screen.height * .15f), 
			               "Yes!", styleLibrary.MainStyles.Button))
            {
                SaveData.UnlockedGods = new List<ShopControl.Gods> { ShopControl.Gods.Buluc, ShopControl.Gods.Chac };
                SaveData.UnlockedCards = new List<LibraryCard>();
                SaveLoad.Save();
                MainMenuUp = true;
                DeleteDataMenuUp = false;
                SaveData.NewSaveFile();
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
            GUI.Box(new Rect(Screen.width * .1f, Screen.height * .2f, Screen.width * .8f, Screen.height * .3f), 
			        "You died!", styleLibrary.MainStyles.Title);
            GUI.Box(new Rect(Screen.width * .1f, Screen.height * .55f, Screen.width * .8f, Screen.height * .15f), 
			        "Got to level " + GameControl.Level.ToString(), styleLibrary.MainStyles.DiedAndGotToLevel);
            if (UnlockedText != "")
            {
                GUI.Box(new Rect(Screen.width * .1f, Screen.height * .55f, Screen.width * .8f, Screen.height * .15f), 
				        UnlockedText, styleLibrary.MainStyles.Title);
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
