﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EncyclopediaMenu : MonoBehaviour {

	ShopControl shopControl;
	ShopControlGUI shopControlGUI;
	GUIStyleLibrary styleLibrary;
	MenuControl menuControl;

	bool[] Tabs = {false, false, false, false};
	int SelectedGod;

	List<Goal> shownGoals = new List<Goal>();
	int selectedGoal;
	List<LibraryCard> shownCards = new List<LibraryCard>();
	List<string> UnlockedCardNames = new List<string>();
	int selectedCard;
	List<EnemyLibraryCard> allEnemies = new List<EnemyLibraryCard> ();
	public Texture2D[] enemyPortraitTextures;
	public Texture2D[] enemyAttackTextures;

	Vector2 scrollPos = Vector2.zero;
	string[] TabStrings = new string[] {"Gods", "Goals", "Cards", "Enemies"};
	
	void Start() {
		shopControlGUI = gameObject.GetComponent<ShopControlGUI> ();
		shopControl = gameObject.GetComponent<ShopControl> ();
		styleLibrary = gameObject.GetComponent<GUIStyleLibrary> ();
		menuControl = gameObject.GetComponent<MenuControl> ();
	}

	void OnGUI() {
		GUI.depth = 1;
		
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", styleLibrary.MainStyles.BlackBackground);

		GUI.depth = 0;

		GUI.BeginGroup(new Rect(Screen.width*.1f, Screen.height*.051f, Screen.width*.8f, Screen.height*.91f), "");

		if(GUI.Button(new Rect(0, Screen.height*.81f, Screen.width*.8f, Screen.height*.1f), 
		              "Go back", styleLibrary.EncyclopediaStyles.BackButton)) {
			menuControl.TurnOnMenu(MenuControl.MenuType.MainMenu);
		}

		for(int i = 0; i < TabStrings.Length; i++) {
			GUIStyle buttonStyle = new GUIStyle(styleLibrary.EncyclopediaStyles.TabOff);
			if(Tabs[i]) buttonStyle = new GUIStyle(styleLibrary.EncyclopediaStyles.TabOn);
			buttonStyle.fontSize = styleLibrary.EncyclopediaStyles.SlightlyBiggerTextFontSize;
			if(GUI.Button(new Rect(Screen.width*(.2f*i), 0, Screen.width*.2f, Screen.height*.09f), TabStrings[i], buttonStyle)) {
				shownGoals = new List<Goal>();
				shownCards = new List<LibraryCard>();
				FindEnemies();
				selectedGoal = -1;
				selectedCard = -1;
				for(int j = 0; j < Tabs.Length; j++) {
					if(i == j) Tabs[j] = true;
					else Tabs[j] = false;
				}
				if(shopControl == null) shopControl = gameObject.GetComponent<ShopControl>();
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
				if(GUI.Button(new Rect(Screen.width*.8f/7*i, Screen.height*.1f, Screen.width*.8f/7, Screen.height*.09f), 
				              shopControlGUI.GodIcons[i], godIconStyle)){
					SelectedGod = i;
					selectedCard = -1;
					selectedGoal = -1;
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
				GUIStyle RedirectButton = new GUIStyle(styleLibrary.EncyclopediaStyles.TabOn);
				RedirectButton.fontSize = styleLibrary.EncyclopediaStyles.RedirectButtonFontSize;
				GUI.DrawTexture(new Rect(0,Screen.height*.2f,Screen.width*.38f, Screen.height*.4f), 
				                shopControlGUI.GodFullTextures[SelectedGod]);
				GUIStyle GodDescription = new GUIStyle(styleLibrary.EncyclopediaStyles.NeutralButton);
				GodDescription.fontSize = styleLibrary.EncyclopediaStyles.GodFontDescriptionSize;
				GodDescription.alignment = TextAnchor.UpperLeft;
				GodDescription.padding = new RectOffset(12, 12, 12, 12);
				GUI.Box(new Rect(Screen.width*.4f, Screen.height*.2f, Screen.width*.4f, Screen.height*.4f), 
				        ShopControl.GodDescriptions[SelectedGod], GodDescription);
				if(GUI.Button(new Rect(0, Screen.height*.65f, Screen.width*.35f, Screen.height*.1f), 
				              "Go to " + godName + "'s Goals", RedirectButton)) {
					Tabs[0] = false;
					Tabs[1] = true;
				}
				if(GUI.Button(new Rect(Screen.width*.45f, Screen.height*.65f, Screen.width*.35f, Screen.height*.1f), 
				              "Go to " + godName + "'s cards", RedirectButton)) {
					Tabs[0] = false;
					Tabs[2] = true;
				}
			}
			else if(Tabs[1]) {
				if(shownGoals.Count == 0 && selectedGoal != -1) {
					FindGoals();
				}

				scrollPos = GUI.BeginScrollView(new Rect(Screen.width*.6f, Screen.height*.2f, Screen.width*.4f, Screen.height*.59f), scrollPos, 
				                new Rect(Screen.width*.6f, Screen.height*.2f, Screen.width*.2f, Screen.height*(.1f*shownGoals.Count)));
//		NoneStyleWordWrap.fontSize = 
				for(int i = 0; i < shownGoals.Count; i++) {
					GUIStyle goalStyle = new GUIStyle(styleLibrary.EncyclopediaStyles.TabOff);
					if(selectedGoal == i) goalStyle = new GUIStyle(styleLibrary.EncyclopediaStyles.TabOn);
					goalStyle.fontSize = styleLibrary.EncyclopediaStyles.GoalFontSize;
					if(GUI.Button(new Rect(Screen.width*.6f,Screen.height*.1f*(i+2), Screen.width*.18f, Screen.height*.1f), 
					              shownGoals[i].MiniDescription, goalStyle)){
						selectedGoal = i;
					}
				}
				GUI.EndScrollView();
				
				if(selectedGoal == -1) {
					GUI.Box(new Rect(0, Screen.height*.2f, Screen.width*.6f, Screen.height*.4f), 
					        "Pick a goal to learn more", styleLibrary.EncyclopediaStyles.InfoBox);
				}
				else {
					GUIStyle goalTitleStyle = new GUIStyle(styleLibrary.EncyclopediaStyles.NeutralButton);
					goalTitleStyle.fontSize = styleLibrary.EncyclopediaStyles.SlightlyBiggerTextFontSize;
					GUI.Box(new Rect(Screen.width*.1f, Screen.height*.2f, Screen.width*.4f, Screen.height*.2f), 
					        shownGoals[selectedGoal].MiniDescription, goalTitleStyle);
					GUI.Box(new Rect(0, Screen.height*.45f, Screen.width*.3f, Screen.height*.3f), 
					        "", styleLibrary.EncyclopediaStyles.NeutralButton);
					GUI.Box(new Rect(Screen.width*.02f, Screen.height*.45f, Screen.width*.2f, Screen.height*.3f),
					        shownGoals[selectedGoal].GoalScore[0] + "\n" + 
					        shownGoals[selectedGoal].GoalScore[1] + "\n" + 
					        shownGoals[selectedGoal].GoalScore[2], styleLibrary.EncyclopediaStyles.BigText);
					GUI.DrawTexture(new Rect(Screen.width*.175f, Screen.height*.47f, Screen.width*.1f, Screen.height*.24f),
					                shopControlGUI.STOPLIGHTTEXTURE);
					if(SaveDataControl.GoalHighScores.ContainsKey(shownGoals[selectedGoal].MiniDescription)) {
						GUI.Box(new Rect(Screen.width*.3f,Screen.height*.45f, Screen.width*.3f, Screen.height*.3f), 
						        "Your high score is " + SaveDataControl.GoalHighScores[shownGoals[selectedGoal].MiniDescription].ToString(), 
						        styleLibrary.EncyclopediaStyles.NeutralButton);
					} else {
						GUI.Box(new Rect(Screen.width*.3f,Screen.height*.45f, Screen.width*.3f, Screen.height*.3f), 
						        "You don't have a high score for this goal.", 
						        styleLibrary.EncyclopediaStyles.NeutralButton);					}
				}
			}
			else if(Tabs[2]) {
				if(shownCards.Count == 0 && selectedCard != -1) {
					FindCards();
				}
				
				scrollPos = GUI.BeginScrollView(new Rect(Screen.width*.6f, Screen.height*.2f, Screen.width*.2f, Screen.height*.6f), scrollPos, 
				                                new Rect(Screen.width*.6f, Screen.height*.2f, Screen.width*.15f, Screen.height*.1f*shownCards.Count));
				for(int i = 0; i < shownCards.Count; i++) {
					if(UnlockedCardNames.Contains(shownCards[i].CardName)) {

						GUIStyle CardTabStyle = new GUIStyle(styleLibrary.EncyclopediaStyles.TabOff);
						if(selectedCard == i) CardTabStyle = new GUIStyle(styleLibrary.EncyclopediaStyles.TabOn);
						if(shownCards[i].ThisRarity == Card.Rarity.Paper) {
							CardTabStyle.normal = styleLibrary.EncyclopediaStyles.NeutralButton.normal;
							CardTabStyle.active = styleLibrary.EncyclopediaStyles.NeutralButton.active;
						}
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
					                shopControlGUI.CardTextures[SelectedGod]);
					GUIStyle cardNameStyle = new GUIStyle(styleLibrary.EncyclopediaStyles.CardNameStyle);
					GUIStyle cardTextStyle = new GUIStyle(styleLibrary.EncyclopediaStyles.CardTextStyle);
					cardTextStyle.alignment = TextAnchor.UpperLeft;
					cardTextStyle.fontSize = styleLibrary.EncyclopediaStyles.SlightlyBiggerTextFontSize;
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
					GUI.Box(new Rect(Screen.width*.045f, Screen.height*.4f, Screen.width*.53f, Screen.height*.3f), 
					        shownCards[selectedCard].DisplayText, cardTextStyle);
					GUI.DrawTexture(new Rect(Screen.width*.45f, Screen.height*.7f, Screen.width*.1f, Screen.width*.1f), 
					                shopControlGUI.GodIcons[SelectedGod]);
					Card.Rarity rarity = shownCards[selectedCard].ThisRarity;
					Texture2D rarityTexture = shopControlGUI.PaperTexture;
					if(rarity == Card.Rarity.Bronze) rarityTexture = shopControlGUI.BronzeTexture;
					else if(rarity == Card.Rarity.Silver) rarityTexture = shopControlGUI.SilverTexture;
					else if(rarity == Card.Rarity.Gold) rarityTexture = shopControlGUI.GoldTexture;
					GUI.DrawTexture(new Rect(Screen.width*.05f, Screen.height*.7f, Screen.width*.1f, Screen.width*.1f), rarityTexture);
				}
			}
		}
		else if (Tabs[3]) {
//			scrollPos = GUI.BeginScrollView(new Rect(Screen.width*.6f, Screen.height*.2f, Screen.width*.4f, Screen.height*.6f), scrollPos, 
//			                                new Rect(Screen.width*.6f, Screen.height*.2f, Screen.width*.2f, Screen.height*.1f*shownGoals.Count));
			scrollPos = GUI.BeginScrollView(new Rect(0, Screen.height*.11f, Screen.width*.8f, Screen.height*.68f), scrollPos, 
			                                new Rect(0, Screen.height*.11f, Screen.width*.6f, Screen.height*.15f*allEnemies.Count));
			for(int i = 0; i < allEnemies.Count; i++) {
				if(SaveDataControl.DefeatedEnemies.Contains(allEnemies[i].Name)) {					
					GUI.BeginGroup(new Rect(0,Screen.height*(.15f*(i+1) - .04f), Screen.width*.75f, Screen.height*.14f), 
					        	   styleLibrary.EncyclopediaStyles.NeutralButton);
					if(enemyPortraitTextures[i] != null) {
						GUI.DrawTexture(new Rect(Screen.height*.01f, Screen.height*.02f, Screen.height*.10f, Screen.height*.10f), enemyPortraitTextures[i]);
					}
					GUI.Box (new Rect(Screen.width*.21f, Screen.height*.005f,Screen.width*.38f, Screen.height*.09f), 
					         allEnemies[i].EncyclopediaEntry, styleLibrary.EncyclopediaStyles.NoneStyleWordWrap);
					GUI.DrawTexture (new Rect(Screen.width*.593f, Screen.width*.0325f, Screen.width*.14f, Screen.width*.14f), enemyAttackTextures[i]);
					GUI.Box(new Rect(Screen.width*.6f, Screen.width*.18f, Screen.width*.14f, Screen.width*.4f), 
					        "Lv. " + allEnemies[i].ChallengeRating, styleLibrary.EncyclopediaStyles.NoneStyleWordWrap);
					GUI.EndGroup();
				}
				else {
					GUIStyle tabOffNoHover = new GUIStyle(styleLibrary.EncyclopediaStyles.TabOff);
					tabOffNoHover.hover = new GUIStyleState();
					GUI.Box(new Rect(0,Screen.height*(.15f*(i+1) - .04f), Screen.width*.75f, Screen.height*.14f),
					        "???", tabOffNoHover);
				}
			}
			GUI.EndScrollView();
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
	}
	void FindCards() {
		UnlockedCardNames = new List<string> ();
		for(int i = 0; i < SaveDataControl.UnlockedCards.Count; i++) {
			UnlockedCardNames.Add(SaveDataControl.UnlockedCards[i].CardName);
		}

		shownCards = new List<LibraryCard> ();

		LibraryCard[] bunch = new LibraryCard[CardLibrary.Lib.Count];
		CardLibrary.Lib.Values.CopyTo (bunch, 0);

		for(int i = 0; i < bunch.Length; i++) {
			if(bunch[i].God == ShopControl.AllGods[SelectedGod]) {
				shownCards.Add(bunch[i]);
			}
		}
	}
	void FindEnemies() {
		EnemyLibraryCard[] bunch = new EnemyLibraryCard[EnemyLibrary.Lib.Count];
		EnemyLibrary.Lib.Values.CopyTo (bunch, 0);
		enemyPortraitTextures = new Texture2D[bunch.Length];
		enemyAttackTextures = new Texture2D[bunch.Length];
		allEnemies = new List<EnemyLibraryCard> ();
		for(int i = 0; i < bunch.Length; i++) {
			allEnemies.Add(bunch[i]);
			enemyPortraitTextures[i] = Resources.Load<Texture2D> (bunch[i].SpritePath);
			enemyAttackTextures[i] = (Texture2D)Resources.Load("sprites/targeting icons/range " + bunch[i].AttackTargetType.ToString() + 
			                                                " " + bunch[i].AttackMinRange.ToString() + "-" + bunch[i].AttackMaxRange.ToString());
		}
	}
}
