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

	GameControl battleBoss;
	ShopControl shopBoss;
	static ClickControl clickBoss;

	public static string errorText = "";

	static ShopControl.Gods[] UnlockOrder = new ShopControl.Gods[] {ShopControl.Gods.Chac, ShopControl.Gods.Ikka, ShopControl.Gods.Ixchel, 
	                                                                ShopControl.Gods.Kinich, ShopControl.Gods.Akan, ShopControl.Gods.Ekcha};
	static int[] UnlockLevels = new int[6] {2, 4, 6, 8, 10, 12};
	EncyclopediaMenu encyclopediaMenu;

	public GUISkin MAINMENUGUISKIN;

	void Start() {
		GameObject gameController = GameObject.FindGameObjectWithTag ("GameController");
		battleBoss = gameController.GetComponent<GameControl> ();
		clickBoss = gameController.GetComponent<ClickControl> ();
		shopBoss = gameController.GetComponent<ShopControl> ();
		encyclopediaMenu = gameController.GetComponent<EncyclopediaMenu> ();
	}

    //Could be called by MainMenu or by GodChoiceMenu, or by DeckAnimate. starts the game or goes to the next level.
    public void BeginButton()
    {
		MainMenu.errorText = "";

		GameObject gameController = GameObject.FindGameObjectWithTag ("GameController");
		battleBoss = gameController.GetComponent<GameControl> ();

		if(!InGame) {
			battleBoss.BeginGame();
			InGame = true;
		} else {
			battleBoss.StartNewLevel();
		}
	}

    //Brings up the "you died" menu, which is here in MainMenu.
	public static void Die() 
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().alive = false;

        Debug.Log("Died!");

		DiedMenuUp = true;
		clickBoss.DisallowEveryInput ();
		clickBoss.AllowInfoInput = true;
		clickBoss.AllowInputUmbrella = true;
		MainMenu.InGame = false;

        GameControl battleBoss = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        battleBoss.TransformPlayer(false);

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
        ShopControl.Normaldisplay = false;
        UnlockedText = "";
        UnlockCheck();

        GameControl battleBoss = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        battleBoss.DeleteAllCards();

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
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", MAINMENUGUISKIN.box);
            //string text = "Moves are not plays";
            //if (GameControl.MovesArePlays) 
            //    text = "Moves are plays";
            //GameControl.MovesArePlays = GUI.Toggle(new Rect(0, 0, Screen.width * .4f, Screen.height * .1f), GameControl.MovesArePlays, text);
        }

        #region Main Menu
        if (MainMenuUp)
        {

            GUI.Box(new Rect(Screen.width * .1f, Screen.height * .05f, Screen.width * .8f, Screen.height * .15f), "Popul Vuh", MAINMENUGUISKIN.customStyles[0]);

            //Start new game
            if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .25f, Screen.width * .6f, Screen.height * .1f), "Start new game", MAINMENUGUISKIN.button))
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
            //		if(GUI.Button(new Rect(Screen.width*.2f, Screen.height*.4f, Screen.width*.6f, Screen.height*.1f), "Unlock cards and gods", MAINMENUGUISKIN.button)) {
            //			UnlockMenu tempmenu = battleBoss.gameObject.GetComponent<UnlockMenu>();
            //			tempmenu.ShowMenu();
            //			MainMenuUp = false;
            //		}

            //Reset unlocked data
            if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .55f, Screen.width * .6f, Screen.height * .1f), "Reset unlocked data", MAINMENUGUISKIN.button))
            {
                DeleteDataMenuUp = true;
                MainMenuUp = false;
            }

            //Customize starting deck
            if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .7f, Screen.width * .6f, Screen.height * .1f), "Customize Starting Deck", MAINMENUGUISKIN.button))
            {
                CustomizeMenu tempMenu = gameObject.GetComponent<CustomizeMenu>();
                tempMenu.OpenMenu();
                MainMenuUp = false;
                SaveData.NewCardsAvailable = false;
            }
            //"New cards available!" window. 
            if (SaveData.NewCardsAvailable)
            {
                GUI.Box(new Rect(Screen.width * .25f, Screen.height * .775f, Screen.width * .5f, Screen.height * .05f), "New cards available!", MAINMENUGUISKIN.customStyles[1]);
            }

            //Encyclopedia
            if (GUI.Button(new Rect(Screen.width * .2f, Screen.height * .85f, Screen.width * .6f, Screen.height * .1f), "Encyclopedia", MAINMENUGUISKIN.button))
            {
                EncyclopediaMenu tempMenu = battleBoss.gameObject.GetComponent<EncyclopediaMenu>();
                tempMenu.ShowMenu();
                MainMenuUp = false;
            }

            //Tutorial
            if (GUI.Button(new Rect(Screen.width * .025f, Screen.height * .3f, Screen.width * .15f, Screen.height * .6f), "Tutorial", MAINMENUGUISKIN.customStyles[4]))
            {
                MainMenuUp = false;
                Tutorial.TutorialLevel = 1;
                BeginButton();
            }

            //Next god to unlock
            GUI.BeginGroup(new Rect(Screen.width * .825f, Screen.height * .3f, Screen.width * .15f, Screen.height * .6f));
            GUI.Box(new Rect(0, Screen.height * .0f, Screen.width * .15f, Screen.height * .2f), "Next Unlock:", MAINMENUGUISKIN.customStyles[4]);
            GUI.Box(new Rect(0, Screen.height * .2f, Screen.width * .15f, Screen.height * .2f), new GUIContent("\n" + NextGodToUnlock.ToString(),
                shopBoss.GodFullTextures[ShopControl.AllGods.IndexOf(NextGodToUnlock)]), MAINMENUGUISKIN.customStyles[4]);
            GUI.Box(new Rect(0, Screen.height * .4f, Screen.width * .15f, Screen.height * .2f),
                "Unlocked at level " + NextUnlockLevel.ToString(), MAINMENUGUISKIN.customStyles[4]);
            GUI.EndGroup();

        }
        #endregion

        #region Delete Data Menu
        if (DeleteDataMenuUp)
        {
            GUI.Box(new Rect(Screen.width * .2f, Screen.height * .1f, Screen.width * .6f, Screen.height * .4f), "Are you sure?", MAINMENUGUISKIN.customStyles[0]);
            if (GUI.Button(new Rect(Screen.width * .6f, Screen.height * .8f, Screen.width * .3f, Screen.height * .15f), "Yes!", MAINMENUGUISKIN.button))
            {
                SaveData.UnlockedGods = new List<ShopControl.Gods> { ShopControl.Gods.Buluc, ShopControl.Gods.Chac };
                SaveData.UnlockedCards = new List<LibraryCard>();
                SaveLoad.Save();
                MainMenuUp = true;
                DeleteDataMenuUp = false;
                SaveData.NewSaveFile();
            }
            if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .8f, Screen.width * .3f, Screen.height * .15f), "Back", MAINMENUGUISKIN.button))
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
            GUI.Box(new Rect(Screen.width * .1f, Screen.height * .2f, Screen.width * .8f, Screen.height * .3f), "You died!", MAINMENUGUISKIN.customStyles[0]);
            GUI.Box(new Rect(Screen.width * .1f, Screen.height * .55f, Screen.width * .8f, Screen.height * .15f), "Got to level " + GameControl.Level.ToString(), MAINMENUGUISKIN.customStyles[3]);
            if (UnlockedText != "")
            {
                GUI.Box(new Rect(Screen.width * .1f, Screen.height * .55f, Screen.width * .8f, Screen.height * .15f), UnlockedText, MAINMENUGUISKIN.customStyles[0]);
            }
            if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .75f, Screen.width * .8f, Screen.height * .15f), "Return to main menu!", MAINMENUGUISKIN.button))
            {
                CleanUpGameboard();
            }
        }
        #endregion
    }
}
