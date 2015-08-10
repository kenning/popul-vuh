using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EncyclopediaMenu : MonoBehaviour {

	public bool EncyclopediaMenuUp = false;
	bool[] Tabs = {false, false, false, false};
	int SelectedGod;

	List<Goal> shownGoals = new List<Goal>();
	int selectedGoal;
	List<LibraryCard> shownCards = new List<LibraryCard>();
	int selectedCard;
	Vector2 scrollPos = Vector2.zero;
	string[] TabStrings = new string[] {"Gods", "Goals", "Cards", "Enemies"};
	List<string> UnlockedCardNames = new List<string>();
	
	ShopControl shopBoss;

	GUIStyleLibrary styleLibrary;

	void Start() {
		styleLibrary = gameObject.GetComponent<GUIStyleLibrary> ();
	}

	void OnGUI() {
		GUI.depth = 0;

		if(!EncyclopediaMenuUp) 
			return;

		GUI.BeginGroup(new Rect(Screen.width*.1f, Screen.height*.05f, Screen.width*.8f, Screen.height*.9f), "");

		if(GUI.Button(new Rect(0, Screen.height*.8f, Screen.width*.8f, Screen.height*.1f), 
		              "Go back", styleLibrary.EncyclopediaStyles.BackButton)) {
			EncyclopediaMenuUp = false;
			MainMenu.MainMenuUp = true;
		}

		for(int i = 0; i < TabStrings.Length; i++) {
			GUIStyle buttonStyle = styleLibrary.EncyclopediaStyles.TabOff;
			if(Tabs[i]) buttonStyle = styleLibrary.EncyclopediaStyles.TabOn;
			if(GUI.Button(new Rect(Screen.width*(.2f*i), 0, Screen.width*.2f, Screen.height*.1f), TabStrings[i], buttonStyle)) {
				shownGoals = new List<Goal>();
				selectedGoal = -1;
				shownCards = new List<LibraryCard>();
				selectedCard = -1;
				for(int j = 0; j < Tabs.Length; j++) {
					if(i == j) Tabs[j] = true;
					else Tabs[j] = false;
				}
				if(shopBoss == null) shopBoss = gameObject.GetComponent<ShopControl>();
				SelectedGod = -1;
				scrollPos = Vector2.zero;
			}
		}

		if(!Tabs[2] && !Tabs[3] && !Tabs[1] && !Tabs[0]) {
			//Intro menu
			GUI.Box(new Rect(Screen.width*.1f, Screen.height*.2f, Screen.width*.6f, Screen.height*.4f), 
			        "Pick a tab to learn more", styleLibrary.EncyclopediaStyles.InfoBox);
		}
		else if(Tabs[2] | Tabs[1] | Tabs[0]) {
				//God tabs
			for(int i = 0; i < ShopControl.AllGods.Count; i++) {
				GUIStyle godIconStyle = styleLibrary.EncyclopediaStyles.TabOff;
				if(SelectedGod == i) godIconStyle = styleLibrary.EncyclopediaStyles.TabOn;
				if(GUI.Button(new Rect(Screen.width*.8f/7*i, Screen.height*.1f, Screen.width*.8f/7, Screen.height*.1f), 
				              shopBoss.GodIcons[i], godIconStyle)){
					SelectedGod = i;
					selectedCard = -1;
					FindGoals();
					FindCards();
				}
			}

			if(SelectedGod == -1) {
				GUI.Box(new Rect(Screen.width*.1f, Screen.height*.2f, Screen.width*.6f, Screen.height*.4f), 
				        "Pick a God to learn more", styleLibrary.EncyclopediaStyles.InfoBox);
				GUI.EndGroup ();
				return;
			}

			if(Tabs[0]) {
				string godName = ShopControl.AllGods[SelectedGod].ToString();
				GUI.DrawTexture(new Rect(0,Screen.height*.2f,Screen.width*.3f, Screen.height*.4f), 
				                shopBoss.GodFullTextures[SelectedGod]);
				GUI.Box(new Rect(Screen.width*.4f, Screen.height*.2f, Screen.width*.4f, Screen.height*.4f), 
				        ShopControl.GodDescriptions[SelectedGod]);
				if(GUI.Button(new Rect(0, Screen.height*.7f, Screen.width*.4f, Screen.height*.1f), 
				              "Go to " + godName + "'s Goals")) {
					Tabs[0] = false;
					Tabs[1] = true;
				}
				if(GUI.Button(new Rect(Screen.width*.4f, Screen.height*.7f, Screen.width*.4f, Screen.height*.1f), 
				              "Go to " + godName + "'s cards")) {
					Tabs[0] = false;
					Tabs[2] = true;
				}
			}
			else if(Tabs[1]) {
				if(shownGoals.Count == 0 && selectedGoal != -1) {
					FindGoals();
				}

				scrollPos = GUI.BeginScrollView(new Rect(Screen.width*.6f, Screen.height*.2f, Screen.width*.4f, Screen.height*.6f), scrollPos, 
				                new Rect(Screen.width*.6f, Screen.height*.2f, Screen.width*.2f, Screen.height*.1f*shownGoals.Count));
				for(int i = 0; i < shownGoals.Count; i++) {
					if(GUI.Button(new Rect(Screen.width*.6f,Screen.height*.1f*(i+2), Screen.width*.18f, Screen.height*.1f), 
					              shownGoals[i].MiniDescription, styleLibrary.EncyclopediaStyles.SubTab)){
						selectedGoal = i;
					}
				}
				GUI.EndScrollView();
				
				if(selectedGoal == -1) {
					GUI.Box(new Rect(0, Screen.height*.2f, Screen.width*.6f, Screen.height*.4f), 
					        "Pick a goal to learn more", styleLibrary.EncyclopediaStyles.InfoBox);
				}
				else {
					GUI.Box(new Rect(Screen.width*.1f, Screen.height*.2f, Screen.width*.4f, Screen.height*.2f), 
					        shownGoals[selectedGoal].MiniDescription);
					GUI.Box(new Rect(0, Screen.height*.45f, Screen.width*.3f, Screen.height*.3f), 
					        "High score info goes here");
						//put high scores here
					GUI.Box(new Rect(Screen.width*.3f,Screen.height*.45f, Screen.width*.3f, Screen.height*.3f), 
					        "High score info goes here");
						//put all time high score here, i guess
				}
			}
			else if(Tabs[2]) {
				if(shownCards.Count == 0 && selectedCard != -1) {
					FindCards();
				}
				
				scrollPos = GUI.BeginScrollView(new Rect(Screen.width*.6f, Screen.height*.2f, Screen.width*.2f, Screen.height*.6f), scrollPos, 
				                                new Rect(Screen.width*.6f, Screen.height*.2f, Screen.width*.17f, Screen.height*.1f*shownCards.Count));
				for(int i = 0; i < shownCards.Count; i++) {
					if(UnlockedCardNames.Contains(shownCards[i].CardName)) {

						GUIStyle CardTabStyle = styleLibrary.EncyclopediaStyles.TabOff;
						if(selectedCard == i) CardTabStyle = styleLibrary.EncyclopediaStyles.TabOn;
						if(GUI.Button(new Rect(Screen.width*.6f,Screen.height*.1f*(i+2), Screen.width*.17f, Screen.height*.1f), 
						              shownCards[i].DisplayName, CardTabStyle)){
							selectedCard = i;
						}
					}
					else {
						GUI.Box(new Rect(Screen.width*.6f,Screen.height*.1f*(i+2), Screen.width*.17f, Screen.height*.1f), 
						        "???", styleLibrary.EncyclopediaStyles.TabOff);
					}
				}
				GUI.EndScrollView();

				if(selectedCard == -1) {
					GUI.Box(new Rect(0, Screen.height*.2f, Screen.width*.6f, Screen.height*.4f), 
					        "Pick a card to learn more", styleLibrary.EncyclopediaStyles.InfoBox);
				}
				else {
					GUI.DrawTexture(new Rect(Screen.width*.0f, Screen.height*.2f, Screen.width*.6f, Screen.height*.6f), 
					                shopBoss.CardTextures[SelectedGod]);
					GUIStyle cardNameStyle = new GUIStyle(styleLibrary.EncyclopediaStyles.CardNameStyle);
					GUIStyle cardTextStyle = new GUIStyle(styleLibrary.EncyclopediaStyles.CardTextStyle);
					if(shownCards[selectedCard].God == ShopControl.Gods.Akan | shownCards[selectedCard].God == ShopControl.Gods.Buluc |
					   shownCards[selectedCard].God == ShopControl.Gods.Ikka | shownCards[selectedCard].God == ShopControl.Gods.Kinich | 
					   shownCards[selectedCard].God == ShopControl.Gods.Chac) {
						cardNameStyle.normal.textColor = Color.black;
						cardTextStyle.normal.textColor = Color.black;
					} else {
						cardNameStyle.normal.textColor = Color.white;
						cardTextStyle.normal.textColor = Color.white;
					}
                    GUI.Box(new Rect(Screen.width*.0f, Screen.height*.2f, Screen.width*.3f, Screen.height*.2f), 
					        shownCards[selectedCard].DisplayName, cardNameStyle);
					Texture2D icon = Resources.Load("sprites/card icons/" + shownCards[selectedCard].IconPath) as Texture2D;
					GUI.Box(new Rect(Screen.width*.325f, Screen.height*.225f, Screen.width*.25f, Screen.height*.15f), icon, GUIStyle.none);
					GUI.Box(new Rect(Screen.width*.025f, Screen.height*.5f, Screen.width*.55f, Screen.height*.3f), 
					        shownCards[selectedCard].DisplayText, cardTextStyle);
					//make this look like display()
					GUI.DrawTexture(new Rect(Screen.width*.45f, Screen.height*.7f, Screen.width*.1f, Screen.width*.1f), 
					                shopBoss.GodIcons[SelectedGod]);
					Card.Rarity rarity = shownCards[selectedCard].ThisRarity;
					Texture2D rarityTexture = shopBoss.PaperTexture;
					if(rarity == Card.Rarity.Copper) rarityTexture = shopBoss.CopperTexture;
					else if(rarity == Card.Rarity.Silver) rarityTexture = shopBoss.SilverTexture;
					else if(rarity == Card.Rarity.Gold) rarityTexture = shopBoss.GoldTexture;
					GUI.DrawTexture(new Rect(Screen.width*.05f, Screen.height*.7f, Screen.width*.1f, Screen.width*.1f), rarityTexture);
				}
			}
		}
		else if (Tabs[3]) {
			GUI.Box(new Rect(Screen.width*.05f, Screen.height*.0f, Screen.width*.7f, Screen.height*.8f), 
			        "I'm gonna do this later because i'm lazy", styleLibrary.EncyclopediaStyles.InfoBox);
		}
		GUI.EndGroup ();
	}

	void FindGoals() {
		shownGoals = new List<Goal> ();
		for(int i = 0; i < GoalLibrary.allGoals.Count; i++) {
			if(GoalLibrary.allGoals[i].God == ShopControl.AllGods[SelectedGod]) {
				shownGoals.Add(GoalLibrary.allGoals[i]);
			}
		}
		Debug.Log (shownGoals.Count.ToString ());
	}
	void FindCards() {
		UnlockedCardNames = new List<string> ();
		for(int i = 0; i < SaveData.UnlockedCards.Count; i++) {
			UnlockedCardNames.Add(SaveData.UnlockedCards[i].CardName);
		}

		shownCards = new List<LibraryCard> ();

		LibraryCard[] bunch = new LibraryCard[CardLibrary.Lib.Count];
		CardLibrary.Lib.Values.CopyTo (bunch, 0);

		for(int i = 0; i < bunch.Length; i++) {
			if(bunch[i].God == ShopControl.AllGods[SelectedGod]) {
				shownCards.Add(bunch[i]);
			}
		}
		Debug.Log (shownCards.Count.ToString ());
	}

	public void ShowMenu() {
		EncyclopediaMenuUp = true;
	}
}
