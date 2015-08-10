using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopControl : MonoBehaviour {

	public GameControl gameControl;
	public ClickControl clickBoss;
	public CardLibrary library;
	ClickBlocker clickBlocker;
	GoalLibrary goalLibrary;
	public GUISkin SHOPSKIN;
	public GUISkin CARDDISPLAYSKIN;

	public Goal[] Goals;
	public bool[] GoalDisplay;
	public static bool Normaldisplay;

	public List<LibraryCard>[] CardsToBuyFrom;

	string[] possibleGoals;

	//Phases
	public bool goalExpo = false;
	bool shopGUI = false;
    public bool shufflin = false;

	public enum Gods {Ikka, Ekcha, Chac, Kinich, Buluc, Ixchel, Akan, Pantheon, none};
	public static string[] GodDescriptions	= {"Alcohols / Discard Pile", "Damage Spells", "Prayers", "Agility", "Protection", "Fire / Card Destruction", "Card draw / Food", "Card draw / Food", "Card draw / Food", "Card draw / Food"};
	public static List<Gods> AllGods = new List<Gods> ();

	public Sprite[] GodFullSprites;
	public Texture2D[] GodFullTextures;

	public Sprite[] SpriteGodIcons;
	public Texture2D[] GodIcons;

	public Sprite[] GodDisplayCards;
	public Texture2D[] CardTextures;
	
	public Sprite[] GodSmallCards;

	public Sprite Paper;
	public Sprite Copper;
	public Sprite Silver;
	public Sprite Gold;
	//public Sprite Platinum;
	public Texture2D PaperTexture;
	public Texture2D CopperTexture;
	public Texture2D SilverTexture;
	public Texture2D GoldTexture;

	public Texture2D STOPLIGHTTEXTURE;
	//public Texture2D PlatinumTexture;

	float shopGUITime = 0f;
	float cardWidth = Screen.width*.3f;
	float cardHeight = Screen.height*.16f;

    string AddedToCollText = "You can add cards in your collection to your starting deck next time you play!";
	
    public void Initialize () {
		gameControl = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameControl>();
		clickBoss =  GameObject.FindGameObjectWithTag ("GameController").GetComponent<ClickControl>();
		library =  GameObject.FindGameObjectWithTag ("GameController").GetComponent<CardLibrary>();
		clickBlocker = GameObject.Find ("moving click blocker").GetComponent<ClickBlocker> ();
		goalLibrary = gameObject.GetComponent<GoalLibrary> ();
		goalLibrary.Startup ();

		Goals = new Goal[0];

		if(AllGods.Count < 5) {
			AllGods.Add (Gods.Akan);
			AllGods.Add (Gods.Buluc);
            AllGods.Add (Gods.Chac);
            AllGods.Add (Gods.Ekcha);
			AllGods.Add (Gods.Ikka);
			AllGods.Add (Gods.Ixchel);
			AllGods.Add (Gods.Kinich);
			AllGods.Add (Gods.Pantheon);
			AllGods.Add (Gods.none);
		}

        goalExpo = false;
	}

	void OnGUI () {

        #region Initial goal interface
		if(goalExpo) {
			GUI.BeginGroup(new Rect(0,0,Screen.width,Screen.height), "", SHOPSKIN.customStyles[0]);

			for(int i = 0; i < Goals.Length; i++) {
				GUI.Box(new Rect(Screen.width*.1f, Screen.height*(.1f + i*.2f), Screen.width*.2f, Screen.height*.18f), (Texture2D)Goals[i].GodTexture, GUIStyle.none);
				GUI.Box(new Rect(Screen.width*.3f, Screen.height*(.1f + i*.2f), Screen.width*.6f, Screen.height*.18f), Goals[i].GodString + Goals[i].Description, SHOPSKIN.box);
			}

			if(GUI.Button(new Rect(Screen.width*.2f, Screen.height*.8f, Screen.width*.6f, Screen.height*.1f), "Got it!", SHOPSKIN.button)) {
				goalExpo = false;
				clickBoss.Invoke("AllowEveryInput", .1f);
				SetGoalGUI();
			}

			GUI.EndGroup();
        }
        #endregion

        #region Shop interface
        else if (shopGUI) {
			GUI.Box(new Rect(0,0,Screen.width, Screen.height), "", SHOPSKIN.customStyles[0]);
			GUI.BeginGroup(new Rect(Screen.width*.05f, Screen.height*.04f, Screen.width*.9f, Screen.height*.81f));
			for(int i = 0; i < CardsToBuyFrom.Length; i++) {
			//box with god and goal
				GUI.BeginGroup(new Rect(cardWidth*i, 0, cardWidth, cardHeight));
					//god picture
					GUI.Box(new Rect(0,0,Screen.width*.3f, Screen.height*.1f), (Texture2D)Goals[i].GodTexture);
					//grade and box color
					string grade = "Nothing! +$0";
					GUIStyle thisStyle = SHOPSKIN.customStyles[2];
					if(Goals[i].HigherScoreIsGood) {
						if(Goals[i].HighScore >= Goals[i].GoalScore[2]) {
							grade = "Gold! +$3";
							thisStyle = SHOPSKIN.customStyles[6];
						} else if (Goals[i].HighScore >= Goals[i].GoalScore[1]) {
							grade = "Silver! +$2";
							thisStyle = SHOPSKIN.customStyles[5];
						} else if(Goals[i].HighScore >= Goals[i].GoalScore[0]) {
							grade = "Bronze! +$1";
							thisStyle = SHOPSKIN.customStyles[4];
						}
					} else {
						if(Goals[i].HighScore <= Goals[i].GoalScore[2]) { 
							grade = "Gold! +$3";
							thisStyle = SHOPSKIN.customStyles[6];
						} else if(Goals[i].HighScore <= Goals[i].GoalScore[1]) {
							grade = "Silver! +$2";
							thisStyle = SHOPSKIN.customStyles[5];
						} else if(Goals[i].HighScore <= Goals[i].GoalScore[0]) { 
							grade = "Bronze! +$1";
							thisStyle = SHOPSKIN.customStyles[4];
						}
					}
					if(grade != "Nothing! +$0") GUI.Box(new Rect(0, Screen.height*.105f, Screen.width*.3f, Screen.height*.05f), grade, thisStyle);
				GUI.EndGroup();

            //cards to buy
				for(int j = 0; j < CardsToBuyFrom[i].Count; j++) {
					LibraryCard thisCard = CardsToBuyFrom[i][j];

					if(thisCard == null) 
						break;

					if(shopGUITime != 0) {
						GUI.color = new Color(1,1,1,(Time.time-shopGUITime-.5f));
					}
					GUIStyle backgroundStyle = new GUIStyle();
					backgroundStyle.normal.background = CardTextures[AllGods.IndexOf(thisCard.God)];
                    GUI.BeginGroup(new Rect(cardWidth*i, (j+1f)*cardHeight, cardWidth*.8f, cardHeight), "", backgroundStyle);

					GUIStyle cardNameStyle = new GUIStyle(CARDDISPLAYSKIN.customStyles[6]);
					cardNameStyle.fontSize = 14;
					cardNameStyle.alignment = TextAnchor.UpperLeft;
					GUIStyle cardTextStyle = new GUIStyle(CARDDISPLAYSKIN.customStyles[7]);
					cardTextStyle.fontSize = 10;
					cardTextStyle.alignment = TextAnchor.UpperLeft;
					
					if(thisCard.God == ShopControl.Gods.Akan | thisCard.God == ShopControl.Gods.Buluc |
					   thisCard.God == ShopControl.Gods.Ikka | thisCard.God == ShopControl.Gods.Kinich | 
					   thisCard.God == ShopControl.Gods.Chac) {
						cardNameStyle.normal.textColor = Color.black;
						cardTextStyle.normal.textColor = Color.black;
					} else {
						cardNameStyle.normal.textColor = Color.white;
						cardTextStyle.normal.textColor = Color.white;
					}

					Texture2D icon = Resources.Load("sprites/card icons/" + thisCard.IconPath) as Texture2D;
					GUI.DrawTexture(new Rect(cardWidth*.42f, cardHeight*.05f, cardWidth*.3f, cardWidth*.3f), icon);

					GUI.Box(new Rect(cardWidth*.05f,cardHeight*.05f,cardWidth*.35f, cardHeight*.3f), thisCard.CardName, cardNameStyle);
					GUI.Box(new Rect(cardWidth*.05f,cardHeight*.4f, cardWidth*.7f, cardHeight*.5f), thisCard.DisplayText, cardTextStyle);

					Card.Rarity rarity = thisCard.ThisRarity;
					Texture2D rarityTexture = PaperTexture;
					if(rarity == Card.Rarity.Copper) rarityTexture = CopperTexture;
					else if(rarity == Card.Rarity.Silver) rarityTexture = SilverTexture;
					else if(rarity == Card.Rarity.Gold) rarityTexture = GoldTexture;
					GUI.DrawTexture(new Rect(cardWidth*.05f, cardHeight*.85f, cardWidth*.1f, cardWidth*.1f), rarityTexture);

					int SelectedGod = AllGods.IndexOf(thisCard.God);
					GUI.DrawTexture(new Rect(cardWidth*.65f, cardHeight*.85f, cardWidth*.1f, cardWidth*.1f), GodIcons[SelectedGod]);

					GUIStyle costBox = new GUIStyle();
					if(thisCard.ThisRarity == Card.Rarity.Copper) costBox = new GUIStyle(SHOPSKIN.customStyles[4]);
					if(thisCard.ThisRarity == Card.Rarity.Silver) costBox = new GUIStyle(SHOPSKIN.customStyles[5]);
					if(thisCard.ThisRarity == Card.Rarity.Gold) costBox = new GUIStyle(SHOPSKIN.customStyles[6]);
					costBox.fontSize = 22;

                //Hover highlight button. click to buy the card. drawn after card so it's on top!
                    string willUnlock = "";
                    if (!SaveData.UnlockedCards.Contains(thisCard) && SaveData.UnlockedGods.Contains(thisCard.God))
                    {
                        willUnlock = "Buying this card will add it to your collection!";
                    }
                    if (gameControl.Dollars >= thisCard.Cost)
                    {

                        if (GUI.Button(new Rect(0, 0, cardWidth*.8f, cardHeight), willUnlock, SHOPSKIN.customStyles[3]))
                        {
                            if (gameControl.Dollars >= thisCard.Cost)
                            {
                                string tempString = thisCard.CardName.ToString();
                                gameControl.Deck.Add(tempString);
                                gameControl.AddDollars(-thisCard.Cost);
                                CardsToBuyFrom[i].RemoveAt(j);



                                if (!SaveData.UnlockedCards.Contains(thisCard) && SaveData.UnlockedGods.Contains(thisCard.God))
                                {
                                    AddedToCollText = "Added " + thisCard.God.ToString() + "'s card " + tempString + " to your collection!\n" + AddedToCollText;
                                    //Don't just add it! call the method 
                                    SaveData.AddCardToUnlocked(thisCard);
                                    SaveLoad.Save();
                                }
                            }
                            else
                            {
                                Debug.Log("Not enough money");
                            }
                        }
                    }
                    GUI.EndGroup();
                    GUI.Box(new Rect(cardWidth * i + cardWidth * .8f, (j + 1f) * cardHeight, cardWidth * .2f, cardHeight), "$\n" + thisCard.Cost.ToString(), costBox);
				}
                
                //this is to limit the fading to this element only
				if(Time.time != 0) {
					GUI.color = new Color(1,1,1,1);
					if(Time.time-shopGUITime > 1) {
						shopGUITime = 0;
						GUI.color = new Color(1,1,1,1);
                    }
				}
            }
            GUI.EndGroup();

        //"You added x to your collection" box
            if (AddedToCollText != "You can add cards in your collection to your starting deck next time you play!")
            {
                GUI.Box(new Rect(Screen.width * .1f, Screen.height * .7f, Screen.width * .8f, Screen.height * .15f), AddedToCollText, SHOPSKIN.customStyles[2]);
            }

        //Dollar count box
			GUI.Box(new Rect(Screen.width*.7f, Screen.height*.88f, Screen.width*.2f, Screen.height*.1f), gameControl.Dollars.ToString(), SHOPSKIN.button);

        //Go to next level button
			if(GUI.Button(new Rect(Screen.width*.2f, Screen.height*.88f, Screen.width*.4f, Screen.height*.1f), "Go to next level", SHOPSKIN.button)) {
				shopGUI = false;
				gameControl.CollectAnimate();
                gameControl.Tooltip = "Shuffling together your deck and discard...";
                AddedToCollText = "You can add cards in your collection to your starting deck next time you play!";
                shufflin = true; //this bool prevents the normal goal display while shuffle animating.
			}
        }
        #endregion

        #region Normal gui interface (clickable boxes on top that show the 3 goals)
        else if (Normaldisplay && !shufflin) {
			for(int i = 0; i < Goals.Length; i++) {
				GUI.Box(new Rect(Screen.width*i*.333333f, 0, Screen.width*.11111f, Screen.height*.05f), Goals[i].GodIcon, SHOPSKIN.textArea);

				if(GoalDisplay[i]){
					GUI.Box(new Rect(Screen.width*(i*.333333f),Screen.height*.05f, Screen.width*.333333f, Screen.height*.15f), Goals[i].GodString + Goals[i].Description, SHOPSKIN.textArea);
					GUI.Box(new Rect(Screen.width*(i*.333333f + .11111f), 0, Screen.width*.22222f, Screen.height*.05f), Goals[i].CurrentScore.ToString(), SHOPSKIN.textArea);
					GUI.Box(new Rect(Screen.width*(i*.333333f + .22222f), Screen.height*.2f, Screen.width*.11111f, Screen.height*.1f), STOPLIGHTTEXTURE, SHOPSKIN.textArea);
					string tempString = "";
					for(int j = 0; j < Goals[i].GoalScore.Length; j++){

						if(Goals[i].HigherScoreIsGood) {
							if(Goals[i].HighScore >= Goals[i].GoalScore[j]) tempString += "X " + Goals[i].GoalScore[j].ToString();
							else tempString += "  " + Goals[i].GoalScore[j].ToString();
							//this is for hiding platinum goals VVVV
							//if(j+2 == Goals[i].GoalScore.Length) break;
							if(j+1 != Goals[i].GoalScore.Length) tempString += "\n";
						}
						else {
							if(Goals[i].HighScore <= Goals[i].GoalScore[j]) tempString += "X " + Goals[i].GoalScore[j].ToString();
							else tempString += "  " + Goals[i].GoalScore[j].ToString();
							//for hiding platinum goals
							//if(j+2 == Goals[i].GoalScore.Length) break;
							if(j+1 != Goals[i].GoalScore.Length) tempString += "\n";
						}
					}
					GUI.Box(new Rect(Screen.width*(i*.333333f), Screen.height*.2f, Screen.width*.13333333333f, Screen.height*.1f), "Best score:\n" + Goals[i].HighScore.ToString(), SHOPSKIN.textArea);
					GUI.Box(new Rect(Screen.width*(i*.333333f + .13333333333f), Screen.height*.2f, Screen.width*.1f, Screen.height*.1f), tempString, SHOPSKIN.textArea);

					if(GUI.Button(new Rect(Screen.width*(i*.333333f),0, Screen.width*.333333f, Screen.height*.3f), "", SHOPSKIN.customStyles[1])) {
						GoalDisplay = new bool[] {false, false, false};
						clickBlocker.MoveToSpot(-1);
					}
				}
				else {
					GUI.Box(new Rect(Screen.width*(i*.333333f),Screen.height*.05f, Screen.width*.333333f, Screen.height*.075f), Goals[i].MiniDescription, SHOPSKIN.textArea);
					GUI.Box(new Rect(Screen.width*(i*.333333f + .11111f),0, Screen.width*.22222f, Screen.height*.05f), Goals[i].DisplayScore, SHOPSKIN.textArea);
				}
				if(GUI.Button(new Rect(Screen.width*(i*.333333f),0, Screen.width*.333333f, Screen.height*.125f), "", SHOPSKIN.customStyles[1])){
					GoalDisplay = new bool[]{false, false, false};
					GoalDisplay[i] = true;
					clickBlocker.MoveToSpot(i);
					//bigger shit and then a button at the bottom that closes it
                }
            }
        }
        #endregion
    }
    
    void SetGoalGUI() {
		for(int i = 0; i < Goals.Length; i++) {
			Goals[i].SetDisplayScore();
		}
		shopGUITime = Time.time;
	}

	public void SetGodPicture (Goal goal) {

		int godNumber;
		if(!SaveData.UnlockedGods.Contains(goal.God)) godNumber = 7;
		else godNumber = ShopControl.AllGods.IndexOf (goal.God);

		goal.GodPicture = GodFullSprites [godNumber];
		goal.GodTexture = GodFullTextures [godNumber];
		goal.GodIcon = GodIcons [godNumber];
	}

	public void ProduceCards () {

		SetGoalGUI ();

		CardsToBuyFrom = new List<LibraryCard>[Goals.Length];
		for(int i = 0; i < CardsToBuyFrom.Length; i++) {
			CardsToBuyFrom[i] = new List<LibraryCard>();
		}

		int[] finalScores = FinalScores ();
		for(int i = 0; i < Goals.Length; i++) {

			if(finalScores[i] >= 1) { 
				LibraryCard tempLC = library.PullCardFromPack(Goals[i].God, Card.Rarity.Copper);
				CardsToBuyFrom[i].Add(tempLC);
			}

			if(finalScores[i] == 1) gameControl.AddDollars(1);
			if(finalScores[i] >= 2) CardsToBuyFrom[i].Add(library.PullCardFromPack(Goals[i].God, Card.Rarity.Silver));
			if(finalScores[i] == 2) gameControl.AddDollars(2);
			if(finalScores[i] >= 3) CardsToBuyFrom[i].Add(library.PullCardFromPack(Goals[i].God, Card.Rarity.Gold));
			if(finalScores[i] == 3) gameControl.AddDollars(3);
			if(finalScores[i] >= 4) CardsToBuyFrom[i].Add(library.PullCardFromPack(Goals[i].God, Card.Rarity.Gold));
			if(finalScores[i] == 4) gameControl.AddDollars(5);
		}

		shopGUI = true;
		clickBoss.DisallowEveryInput ();
	}

	int[] FinalScores () {
		int[] finalScores = new int[] {0,0,0};
		for(int i = 0; i < Goals.Length; i++) {
			if(Goals[i].HigherScoreIsGood) {
				for(int j = 0; j < Goals[i].GoalScore.Length; j++) {
					if(Goals[i].HighScore >= Goals[i].GoalScore[j]) finalScores[i]++;
				}
			}
			else {
				for(int j = 0; j < Goals[i].GoalScore.Length; j++) {
					if(Goals[i].HighScore <= Goals[i].GoalScore[j]) finalScores[i]++;
				}
			}
		}
		return finalScores;
	}
	
	public void GoalCheck(string GoalMiniDescription) {
		for(int i = 0; i < Goals.Length; i++){
			if (Goals[i].MiniDescription == GoalMiniDescription) {
				Goals[i].MakeCheck();
			}
		}
	}

	public void NewLevelNewGoals(int numberOfGods) {

		for(int i = 0; i < 3; i++) {
			string clickBlockerName = "click blocker " + (i+1).ToString();
			BoxCollider2D tempClickBlockerCollider = GameObject.Find(clickBlockerName).GetComponent<BoxCollider2D>();
			tempClickBlockerCollider.enabled = (i < numberOfGods);
		}

	//	BoxCollider2D[] clickBlockers = new BoxCollider2D[numberOfGods];
	//	for(int i = 0; i < clickBlockers.Length; i++) {
	//		Debug.Log("this happens, right?");
	//		string clickBlockerName = "click blocker " + (i+1).ToString();
	//		clickBlockers[i] = new BoxCollider2D();
	//		clickBlockers[i] = GameObject.Find(clickBlockerName).GetComponent<BoxCollider2D>();
	//		clickBlockers[i].enabled = true;
	//	}

		GoalDisplay = new bool[numberOfGods];

		Goals = new Goal[numberOfGods];
		Goals = goalLibrary.InitializeGoals (numberOfGods);

		foreach(Goal g in Goals) {
			g.SetGodString();
			SetGodPicture(g);
			g.ResetTheScore();
		}

		SetGoalGUI ();
		goalExpo = true;
		clickBoss.DisallowEveryInput ();

	}
}
