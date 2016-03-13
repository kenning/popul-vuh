using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomizeMenu : MonoBehaviour {

	float seventh = Screen.width * .8f / 7;

	LibraryCard selectedCard = new LibraryCard();
	int selectedGod = -1;
	List<string> UnlockedCardNames = new List<string>();
	List<string> StartingDeckCardNames = new List<string>();
	List<LibraryCard>[] allCards = new List<LibraryCard>[0];
	Vector2 scrollPos = new Vector2();
	string goldSpotTaken = "";
	string BronzeSpotTaken = "";
	string silverSpotTaken = "";
	int longestLength = 0;

	void OnGUI () {
		GUI.depth = 1;
		
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "", S.GUIStyleLibraryInst.MainStyles.BlackBackground);
		
		GUI.depth = 0;

		GUI.BeginGroup (new Rect (Screen.width * .1f, Screen.height * .05f, Screen.width * .9f, Screen.height * .9f));

		if(allCards.Length == 0) {
			FindCards();
		}

		for(int i = 0; i < ShopControl.AllGods.Count-2; i++) {
			if(SaveDataControl.UnlockedGods.Contains(ShopControl.AllGods[i])) {
				GUI.Box(new Rect(seventh*i, 0f, seventh, Screen.height*.1f), new GUIContent(S.ShopControlGUIInst.GodIcons[i]), GUIStyle.none);
			} else {
				GUI.Box(new Rect(seventh*i, 0f, seventh, Screen.height*.1f), new GUIContent(S.ShopControlGUIInst.GodIcons[7]), GUIStyle.none);
			}
		}
		
		scrollPos = GUI.BeginScrollView(new Rect(0, Screen.height*.1f, Screen.width*.9f, Screen.height*.47f), scrollPos, 
		                                new Rect(0, Screen.height*.1f, Screen.width*.795f, Screen.height*.1f*longestLength));
		for(int i = 0; i < ShopControl.AllGods.Count; i++) {

			for(int j = 0; j < allCards[i].Count; j++) {
				LibraryCard thisCard = allCards[i][j];
				Texture2D Rarity = S.ShopControlGUIInst.PaperTexture;
				if(thisCard.ThisRarity == Card.Rarity.Bronze)  Rarity = S.ShopControlGUIInst.BronzeTexture;
				else if(thisCard.ThisRarity == Card.Rarity.Silver)  Rarity = S.ShopControlGUIInst.SilverTexture;
				else if(thisCard.ThisRarity == Card.Rarity.Gold)  Rarity = S.ShopControlGUIInst.GoldTexture;

				GUIContent name = new GUIContent(thisCard.CardName, Rarity);
					//if this isn't unlocked, don't display it....
				if(!UnlockedCardNames.Contains(thisCard.CardName) | 
				   (thisCard.ThisRarity != Card.Rarity.Paper && !SaveDataControl.UnlockedGods.Contains(thisCard.God))) {

					if(GUI.Button(new Rect(seventh*i, Screen.height*.1f*(j+1), seventh, Screen.height*.1f),
					              "???", S.GUIStyleLibraryInst.CustomizeStyles.CardToggleOff)) {

					}
				} 
					//if this is in your starting deck, show a CardToggleRemove button, which removes it
				else if (StartingDeckCardNames.Contains(thisCard.CardName)) {
					GUIStyle UnlockedStyle = new GUIStyle(S.GUIStyleLibraryInst.CustomizeStyles.CardToggleRemove);
					if(thisCard.ThisRarity == Card.Rarity.Paper) {
						UnlockedStyle.normal = S.GUIStyleLibraryInst.CustomizeStyles.CardNeutral.normal;
					}
					if(GUI.Button(new Rect(seventh*i, Screen.height*.1f*(j+1), seventh, Screen.height*.1f), 
					              name, UnlockedStyle)) {
						selectedCard = thisCard;
						selectedGod = i;

						if(thisCard.ThisRarity == Card.Rarity.Paper) {
							return;
						}

						for(int k = 0; k < SaveDataControl.StartingDeckCards.Count; k++) {
							if(SaveDataControl.StartingDeckCards[k].CardName == thisCard.CardName) {
								SaveDataControl.StartingDeckCards.RemoveAt(k);
							}
						}

						FindCards();
					}
				}
					//if this isn't in your starting deck, show a CardToggleAdd button, 
						//which adds it to your starting deck if you haven't added your Bronze or silver card
				else {
					if(GUI.Button(new Rect(seventh*i, Screen.height*.1f*(j+1), seventh, Screen.height*.1f), 
					              name, S.GUIStyleLibraryInst.CustomizeStyles.CardToggleAdd)) {
					
						selectedCard = thisCard;
						selectedGod = i;

                        if (thisCard.ThisRarity == Card.Rarity.Paper) { return; }
                        else
                        {
                            Card.Rarity thisRarity = thisCard.ThisRarity;

                            for (int k = 0; k < SaveDataControl.StartingDeckCards.Count; k++)
                            {
                                if (SaveDataControl.StartingDeckCards[k].ThisRarity == thisRarity)
                                {
                                    SaveDataControl.StartingDeckCards.RemoveAt(k);
                                    break;
                                }
                            }

                            SaveDataControl.StartingDeckCards.Add(thisCard);
                        }
		
						FindCards();
					}
				}
			}
		}
		GUI.EndScrollView ();

		GUI.BeginGroup (new Rect (Screen.width*.4f, Screen.height * .585f, Screen.width * .4f, Screen.height * .3f), "");
        if (BronzeSpotTaken != "")
        {
            if (GUI.Button(new Rect(0, 0, Screen.width * .4f, Screen.height * .1f), 
			               new GUIContent("Your added Bronze card is " + BronzeSpotTaken + " ", S.ShopControlGUIInst.BronzeTexture), 
			               S.GUIStyleLibraryInst.CustomizeStyles.RarityToggleOn))
                selectedCard = CardLibrary.Lib[BronzeSpotTaken];
        }
        else
        {
            GUI.Box(new Rect(0, 0, Screen.width * .4f, Screen.height * .1f), 
			        new GUIContent("You haven't chosen a Bronze card to add to your deck."), S.GUIStyleLibraryInst.CustomizeStyles.RarityToggleOff);
        }
        if (silverSpotTaken != "")
        {
            if (GUI.Button(new Rect(0, Screen.height * .1f, Screen.width * .4f, Screen.height * .1f), 
			               new GUIContent("Your added Silver card is " + silverSpotTaken + " ", S.ShopControlGUIInst.SilverTexture), 
			               S.GUIStyleLibraryInst.CustomizeStyles.RarityToggleOn))
                selectedCard = CardLibrary.Lib[silverSpotTaken];
        }
        else
        {
            GUI.Box(new Rect(0, Screen.height * .1f, Screen.width * .4f, Screen.height * .1f), 
			        new GUIContent("You haven't chosen a silver card to add to your deck."), S.GUIStyleLibraryInst.CustomizeStyles.RarityToggleOff);
        }
        if (goldSpotTaken != "")
        {
            if (GUI.Button(new Rect(0, Screen.height * .2f, Screen.width * .4f, Screen.height * .1f), 
			               new GUIContent("Your added Gold card is " + goldSpotTaken + " ", S.ShopControlGUIInst.GoldTexture), 
			               S.GUIStyleLibraryInst.CustomizeStyles.RarityToggleOn))
                selectedCard = CardLibrary.Lib[goldSpotTaken];
        }
        else
        {
            GUI.Box(new Rect(0, Screen.height * .2f, Screen.width * .4f, Screen.height * .1f), 
			        new GUIContent("You haven't chosen a gold card to add to your deck."), S.GUIStyleLibraryInst.CustomizeStyles.RarityToggleOff);
        }
		GUI.EndGroup ();

		if(selectedCard.CardName != null) {
			GUI.BeginGroup (new Rect (Screen.width * 0f, Screen.height * .585f, Screen.width * .35f, Screen.height * .3f), "");
			if (S.ShopControlGUIInst.CardTextures[selectedGod] != null)
            {
    			GUI.DrawTexture(new Rect(Screen.width*.0f, Screen.height*.0f, Screen.width*.35f, Screen.height*.3f), 
				                S.ShopControlGUIInst.CardTextures[selectedGod]);
            }
			GUIStyle cardNameStyle = new GUIStyle(S.GUIStyleLibraryInst.CustomizeStyles.CardNameStyle);
			cardNameStyle.fontSize = S.GUIStyleLibraryInst.CustomizeStyles.CustomizeCardNameFontSize;
			cardNameStyle.alignment = TextAnchor.UpperLeft;
			GUIStyle cardTextStyle = new GUIStyle(S.GUIStyleLibraryInst.CustomizeStyles.CardTextStyle);
			cardTextStyle.fontSize = S.GUIStyleLibraryInst.CustomizeStyles.CustomizeCardTextFontSize;
			cardTextStyle.alignment = TextAnchor.UpperLeft;
			if(selectedCard.God == ShopControl.Gods.Akan | selectedCard.God == ShopControl.Gods.Buluc |
			   selectedCard.God == ShopControl.Gods.Ikka | selectedCard.God == ShopControl.Gods.Kinich | 
			   selectedCard.God == ShopControl.Gods.Chac) {
				cardNameStyle.normal.textColor = Color.black;
				cardTextStyle.normal.textColor = Color.black;
			} else {
				cardNameStyle.normal.textColor = Color.white;
				cardTextStyle.normal.textColor = Color.white;
			}
			GUI.Box(new Rect(Screen.width*.025f, Screen.height*.025f, Screen.width*.25f, Screen.height*.1f), 
			        selectedCard.DisplayName, cardNameStyle);
			Texture2D icon = Resources.Load("sprites/card icons/" + selectedCard.IconPath) as Texture2D;
			GUI.Box(new Rect(Screen.width*.21f, Screen.height*.025f, Screen.width*.11f, Screen.height*.11f), 
			        icon, GUIStyle.none);
			GUI.Box(new Rect(Screen.width*.025f, Screen.height*.1f, Screen.width*.3f, Screen.height*.15f), 
			        selectedCard.DisplayText, cardTextStyle);
			GUI.DrawTexture(new Rect(Screen.width*.275f, Screen.height*.25f, Screen.width*.05f, Screen.width*.05f), 
			                S.ShopControlGUIInst.GodIcons[selectedGod]);
			Card.Rarity rarity = selectedCard.ThisRarity;
			Texture2D rarityTexture = S.ShopControlGUIInst.PaperTexture;
			if(rarity == Card.Rarity.Bronze) rarityTexture = S.ShopControlGUIInst.BronzeTexture;
			else if(rarity == Card.Rarity.Silver) rarityTexture = S.ShopControlGUIInst.SilverTexture;
			else if(rarity == Card.Rarity.Gold) rarityTexture = S.ShopControlGUIInst.GoldTexture;
			GUI.DrawTexture(new Rect(Screen.width*.025f, Screen.height*.25f, Screen.width*.05f, Screen.width*.05f), rarityTexture);
			GUI.EndGroup ();
		}
		else {
			GUI.Box(new Rect(Screen.width*.0f,Screen.height*.585f, Screen.width*.3f, Screen.height*.3f), 
			        "Your starting deck consists of two of every card of paper rarity.\n\n" +
				"You can pick one unlocked card of each rarity to add to your starting deck.", S.GUIStyleLibraryInst.CustomizeStyles.InstructionInfoBox);
		}


		GUI.EndGroup ();
        
        GUIStyle gobackstyle = new GUIStyle(S.GUIStyleLibraryInst.CustomizeStyles.CardToggleRemove);
        gobackstyle.fontSize = 14;
		if(GUI.Button(new Rect(Screen.width*.3f, Screen.height*.95f, Screen.width*.4f, Screen.height*.05f), 
		              "Go back", gobackstyle)) {
			SaveDataControl.Save();
			selectedCard = new LibraryCard();
			S.MenuControlInst.TurnOnMenu(MenuControl.MenuType.MainMenu);
		}
	}

	public void FindCards() {
		UnlockedCardNames = new List<string> ();
		for(int i = 0; i < SaveDataControl.UnlockedCards.Count; i++) {
			UnlockedCardNames.Add(SaveDataControl.UnlockedCards[i].CardName);
		}

		StartingDeckCardNames = new List<string> ();
		for(int i = 0; i < SaveDataControl.StartingDeckCards.Count; i++) {
			StartingDeckCardNames.Add(SaveDataControl.StartingDeckCards[i].CardName);
		}
		
		allCards = new List<LibraryCard>[ShopControl.AllGods.Count];
		for(int i = 0; i < allCards.Length; i++) {
			allCards[i] = new List<LibraryCard>();
		}
		
		LibraryCard[] bunch = new LibraryCard[CardLibrary.Lib.Count];
		CardLibrary.Lib.Values.CopyTo (bunch, 0);
		
		for(int i = 0; i < bunch.Length; i++) {
			if(bunch[i].God == ShopControl.Gods.none)
				continue;
			int appropriateGod = ShopControl.AllGods.IndexOf(bunch[i].God);
			allCards[appropriateGod].Add(bunch[i]);
			//VV add it again if it's paper.
			if(bunch[i].CardName == "Iron Macana" | bunch[i].CardName == "Wooden Pike" | bunch[i].CardName == "Coffee")
				allCards[appropriateGod].Add(bunch[i]);
		}

		for(int i = 0; i < allCards.Length; i++) {
			if(allCards[i].Count > longestLength) {
				longestLength = allCards[i].Count;
			}
		}

		goldSpotTaken = "";
		silverSpotTaken = "";
		BronzeSpotTaken = "";
		
		for(int k = 0; k < SaveDataControl.StartingDeckCards.Count; k++) {
			if(SaveDataControl.StartingDeckCards[k].ThisRarity == Card.Rarity.Paper) 
				continue;
			else if(SaveDataControl.StartingDeckCards[k].ThisRarity == Card.Rarity.Bronze)
				BronzeSpotTaken = SaveDataControl.StartingDeckCards[k].CardName;
			else if(SaveDataControl.StartingDeckCards[k].ThisRarity == Card.Rarity.Silver) 
				silverSpotTaken = SaveDataControl.StartingDeckCards[k].CardName;
			else if(SaveDataControl.StartingDeckCards[k].ThisRarity == Card.Rarity.Gold) 
				goldSpotTaken = SaveDataControl.StartingDeckCards[k].CardName;
		}
	}
}
