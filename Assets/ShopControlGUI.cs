using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopControlGUI : MonoBehaviour {

	ShopControl shopControl;
	GameControl gameControl;
	ClickControl clickControl;
	GameControlGUI gameControlGUI;
	GUIStyleLibrary styleLibrary;
	public bool goalExpo = false;
	public bool normaldisplay = false;
	public bool shopGUI = false;
	
	public Sprite[] GodFullSprites;
	public Texture2D[] GodFullTextures;
	public Sprite[] SpriteGodIcons;
	public Texture2D[] GodIcons;
	public Sprite[] GodDisplayCards;
	public Texture2D[] CardTextures;
	public Sprite[] GodSmallCards;
	
	public Sprite Paper;
	public Sprite Bronze;
	public Sprite Silver;
	public Sprite Gold;
	public Texture2D PaperTexture;
	public Texture2D BronzeTexture;
	public Texture2D SilverTexture;
	public Texture2D GoldTexture;
	public Texture2D STOPLIGHTTEXTURE;

	Goal[] Goals;
	bool[] GoalDisplay;
	public bool[] highScoreNotification;
	
	public float shopGUITime = 0f;
	float cardWidth = Screen.width*.3f;
	float cardHeight = Screen.height*.16f;

	ClickBlocker clickBlocker;
	
	string AddedToCollText = "";

	void Start () {
		useGUILayout = false;
		gameControl = gameObject.GetComponent<GameControl> ();
		shopControl = gameObject.GetComponent<ShopControl> ();
		clickControl = gameObject.GetComponent<ClickControl> ();
		styleLibrary = gameObject.GetComponent<GUIStyleLibrary> ();
		gameControlGUI = gameObject.GetComponent<GameControlGUI> ();
		clickBlocker = GameObject.Find ("moving click blocker").GetComponent<ClickBlocker> ();

		goalExpo = false;
	}

	public void SetGodPicture (Goal goal) {
		
		int godNumber;
		if(!SaveData.UnlockedGods.Contains(goal.God)) godNumber = 7;
		else godNumber = ShopControl.AllGods.IndexOf (goal.God);
		
		goal.GodPicture = GodFullSprites [godNumber];
		goal.GodTexture = GodFullTextures [godNumber];
		goal.GodIcon = GodIcons [godNumber];
	}

	public void NewLevelNewGoals (int numberOfGods) {
		Goals = shopControl.Goals;
		GoalDisplay = new bool[numberOfGods];
		highScoreNotification = new bool[numberOfGods];

		goalExpo = true;

		for(int i = 0; i < 3; i++) {
			string clickBlockerName = "click blocker " + (i+1).ToString();
			BoxCollider2D tempClickBlockerCollider = GameObject.Find(clickBlockerName).GetComponent<BoxCollider2D>();
			tempClickBlockerCollider.enabled = (i < numberOfGods);
		}
	}

	public void ResetTime() {
		shopGUITime = Time.time;
	}

	public void TurnOnShopGUI() {
		for (int i = 0; i < shopControl.Goals.Length; i++) {
			if(SaveData.CheckForHighScores(shopControl.Goals[i])) {
				highScoreNotification[i] = true;
			}
		}
		shopGUI = true;
	}

	public void TurnOnNormalGUI ()
	{
		normaldisplay = true;
	}
	
	public void TurnOffShopControlGUIs() {
		goalExpo = false;
		shopGUI = false;
		normaldisplay = false;
	}

	public void UnclickGoals() {
		for(int i = 0; i < GoalDisplay.Length; i++) {
			GoalDisplay[i] = false;
		}
	}

	void OnGUI () {
		#region Initial goal interface
		if(goalExpo) {
			GUI.BeginGroup(new Rect(0,0,Screen.width,Screen.height), "", styleLibrary.ShopStyles.TransparentBackground);
			
			for(int i = 0; i < shopControl.Goals.Length; i++) {
				GUI.Box(new Rect(Screen.width*.1f, Screen.height*(.1f + i*.2f), Screen.width*.2f, Screen.height*.18f), 
				        (Texture2D)shopControl.Goals[i].GodTexture, GUIStyle.none);
				GUI.Box(new Rect(Screen.width*.3f, Screen.height*(.1f + i*.2f), Screen.width*.6f, Screen.height*.18f), 
				        shopControl.Goals[i].GodString + shopControl.Goals[i].Description, styleLibrary.ShopStyles.NeutralButton);
			}
			
			if(GUI.Button(new Rect(Screen.width*.2f, Screen.height*.8f, Screen.width*.6f, Screen.height*.1f), 
			              "Got it!", styleLibrary.ShopStyles.GotItButton)) {
				goalExpo = false;
				clickControl.Invoke("AllowEveryInput", .1f);
				shopControl.SetGoalGUIVariables();
				TurnOnNormalGUI();
			}
			
			GUI.EndGroup();
		}
		#endregion
		
		#region Shop interface
		else if (shopGUI) {

			GUI.Box(new Rect(0,0,Screen.width, Screen.height), "", styleLibrary.ShopStyles.TransparentBackground);
			GUI.BeginGroup(new Rect(Screen.width*.05f, Screen.height*.04f, Screen.width*.9f, Screen.height*.81f));
			for(int i = 0; i < shopControl.CardsToBuyFrom.Length; i++) {
				GUI.BeginGroup(new Rect(cardWidth*i, 0, cardWidth, cardHeight));
				GUI.Box(new Rect(0,0,Screen.width*.3f, Screen.height*.1f), (Texture2D)Goals[i].GodTexture);
				string grade = "Nothing! +$0";
				GUIStyle thisStyle = styleLibrary.ShopStyles.ShopBox;
				if(Goals[i].HigherScoreIsGood) {
					if(Goals[i].HighScore >= Goals[i].GoalScore[2]) {
						grade = "Gold! +$3";
						thisStyle = styleLibrary.ShopStyles.ShopGoalGold;
					} else if (Goals[i].HighScore >= Goals[i].GoalScore[1]) {
						grade = "Silver! +$2";
						thisStyle = styleLibrary.ShopStyles.ShopGoalSilver;
					} else if(Goals[i].HighScore >= Goals[i].GoalScore[0]) {
						grade = "Bronze! +$1";
						thisStyle = styleLibrary.ShopStyles.ShopGoalBronze;
					}

				} else {
					if(Goals[i].HighScore <= Goals[i].GoalScore[2]) { 
						grade = "Gold! +$3";
						thisStyle = styleLibrary.ShopStyles.ShopGoalGold;
					} else if(Goals[i].HighScore <= Goals[i].GoalScore[1]) {
						grade = "Silver! +$2";
						thisStyle = styleLibrary.ShopStyles.ShopGoalSilver;
					} else if(Goals[i].HighScore <= Goals[i].GoalScore[0]) { 
						grade = "Bronze! +$1";
						thisStyle = styleLibrary.ShopStyles.ShopGoalBronze;
					}
				}

				GUI.Box(new Rect(0, Screen.height*.105f, Screen.width*.3f, Screen.height*.05f), 
				                                    grade, thisStyle);
				GUI.EndGroup();

				if(highScoreNotification[i]) {
					string scoreText = "New highest score!\n";
					if(!Goals[i].HigherScoreIsGood) scoreText = "New lowest score!\n";
					scoreText += Goals[i].CurrentScore.ToString();
					GUI.Box (new Rect(cardWidth*(.2f+(i)), cardHeight*4.2f, cardWidth*.6f, cardHeight*.9f), 
					         scoreText, styleLibrary.ShopStyles.ShopGoalGold);
				}
				
				//cards to buy
				for(int j = 0; j < shopControl.CardsToBuyFrom[i].Count; j++) {
					LibraryCard thisCard = shopControl.CardsToBuyFrom[i][j];
					
					if(thisCard == null) 
						break;
					
					if(shopGUITime != 0) {
						GUI.color = new Color(1,1,1,(Time.time-shopGUITime-.5f));
					}
					GUIStyle backgroundStyle = new GUIStyle();
					backgroundStyle.normal.background = CardTextures[ShopControl.AllGods.IndexOf(thisCard.God)];
					GUI.BeginGroup(new Rect(cardWidth*i, (j+1f)*cardHeight, cardWidth*.8f, cardHeight), "", backgroundStyle);
					
					GUIStyle cardNameStyle = new GUIStyle(styleLibrary.ShopStyles.DisplayTitle);
					cardNameStyle.fontSize = styleLibrary.ShopStyles.DisplayTitleFontSize;
					cardNameStyle.alignment = TextAnchor.UpperLeft;
					GUIStyle cardTextStyle = new GUIStyle(styleLibrary.ShopStyles.DisplayText);
					cardTextStyle.fontSize = styleLibrary.ShopStyles.DisplayTextFontSize;
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
					GUI.Box(new Rect(cardWidth*.05f,cardHeight*.4f, cardWidth*.7f, cardHeight*.5f), thisCard.MiniDisplayText, cardTextStyle);
					
					Card.Rarity rarity = thisCard.ThisRarity;
					Texture2D rarityTexture = PaperTexture;
					if(rarity == Card.Rarity.Bronze) rarityTexture = BronzeTexture;
					else if(rarity == Card.Rarity.Silver) rarityTexture = SilverTexture;
					else if(rarity == Card.Rarity.Gold) rarityTexture = GoldTexture;
					GUI.DrawTexture(new Rect(cardWidth*.05f, cardHeight*.85f, cardWidth*.1f, cardWidth*.1f), rarityTexture);
					
					int SelectedGod = ShopControl.AllGods.IndexOf(thisCard.God);
					GUI.DrawTexture(new Rect(cardWidth*.65f, cardHeight*.85f, cardWidth*.1f, cardWidth*.1f), GodIcons[SelectedGod]);
					
					GUIStyle costBox = new GUIStyle();
					if(thisCard.ThisRarity == Card.Rarity.Bronze) costBox = new GUIStyle(styleLibrary.ShopStyles.ShopGoalBronze);
					if(thisCard.ThisRarity == Card.Rarity.Silver) costBox = new GUIStyle(styleLibrary.ShopStyles.ShopGoalSilver);
					if(thisCard.ThisRarity == Card.Rarity.Gold) costBox = new GUIStyle(styleLibrary.ShopStyles.ShopGoalGold);
					costBox.fontSize = 22;
					
					//Hover highlight button. click to buy the card. drawn after card so it's on top!
					string willUnlock = "";
					if (!SaveData.UnlockedCards.Contains(thisCard) && SaveData.UnlockedGods.Contains(thisCard.God))
					{
						willUnlock = "Buying this card will add it to your collection!";
					}
					if (gameControl.Dollars >= thisCard.Cost)
					{
						
						if (GUI.Button(new Rect(0, 0, cardWidth*.8f, cardHeight), 
						               willUnlock, styleLibrary.ShopStyles.ShopHoverOverlay))
						{
							if (gameControl.Dollars >= thisCard.Cost)
							{
								string tempString = thisCard.CardName.ToString();
								gameControl.Deck.Add(tempString);
								gameControl.AddDollars(-thisCard.Cost);
								shopControl.CardsToBuyFrom[i].RemoveAt(j);

								if(SaveData.TryToUnlockCard(thisCard)) {
									AddedToCollText = "Added " + thisCard.God.ToString() + "'s card " + tempString + 
										" to your collection!\n";
								}
							}
							else
							{
								Debug.Log("Not enough money");
							}
						}
					}
					GUI.EndGroup();
					GUI.Box(new Rect(cardWidth * i + cardWidth * .8f, (j + 1f) * cardHeight, cardWidth * .2f, cardHeight), 
					        "$\n" + thisCard.Cost.ToString(), costBox);
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
			if (AddedToCollText != "")
			{
				GUI.Box(new Rect(Screen.width * .1f, Screen.height * .7f, Screen.width * .8f, Screen.height * .15f), 
				        AddedToCollText + "You can add cards in your collection to your starting deck next time you play!",
				        styleLibrary.ShopStyles.ShopBox);
			}
			
			//Dollar count box
			GUI.Box(new Rect(Screen.width*.7f, Screen.height*.88f, Screen.width*.2f, Screen.height*.1f), 
			        "$" + gameControl.Dollars.ToString(), styleLibrary.ShopStyles.NeutralButton);
			//Go to next level button
			GUIStyle GoToNextLevelStyle = new GUIStyle(styleLibrary.ShopStyles.SmallerButton);
			GoToNextLevelStyle.fontSize = 20;
			if(GUI.Button(new Rect(Screen.width*.2f, Screen.height*.88f, Screen.width*.4f, Screen.height*.1f), 
			              "Go to next level", GoToNextLevelStyle)) {
				shopGUI = false;
				gameControl.CollectAnimate();
				gameControlGUI.SetTooltip("Shuffling together your deck and discard...");
				AddedToCollText = "";
				TurnOffShopControlGUIs(); //this bool prevents the normal goal display while shuffle animating.
			}
		}
		#endregion
		
		#region Normal gui interface (clickable boxes on top that show the 3 goals)
		else if (normaldisplay) {
			for(int i = 0; i < Goals.Length; i++) {
				GUI.Box(new Rect(Screen.width*i*.333333f, 0, Screen.width*.11111f, Screen.height*.05f), 
				        Goals[i].GodIcon, styleLibrary.ShopStyles.InGameGoalBox);
				
				if(GoalDisplay[i]){
					GUI.Box(new Rect(Screen.width*(i*.333333f),Screen.height*.05f, Screen.width*.333333f, Screen.height*.12f), 
					        Goals[i].GodString + Goals[i].Description, styleLibrary.ShopStyles.InGameGoalBox);
					GUI.Box(new Rect(Screen.width*(i*.333333f + .11111f), 0, Screen.width*.22222f, Screen.height*.05f), 
					        Goals[i].CurrentScore.ToString(), styleLibrary.ShopStyles.InGameGoalBox);
					GUI.Box(new Rect(Screen.width*(i*.333333f + .22222f), Screen.height*.17f, Screen.width*.11111f, Screen.height*.1f), 
					        STOPLIGHTTEXTURE, styleLibrary.ShopStyles.InGameGoalBox);
					string tempString = "";
					for(int j = 0; j < Goals[i].GoalScore.Length; j++){
						
						if(Goals[i].HigherScoreIsGood) {
							if(Goals[i].HighScore >= Goals[i].GoalScore[j]) tempString += "X " + Goals[i].GoalScore[j].ToString();
							else tempString += "  " + Goals[i].GoalScore[j].ToString();
							if(j+1 != Goals[i].GoalScore.Length) tempString += "\n";
						}
						else {
							if(Goals[i].HighScore <= Goals[i].GoalScore[j]) tempString += "X " + Goals[i].GoalScore[j].ToString();
							else tempString += "  " + Goals[i].GoalScore[j].ToString();
							if(j+1 != Goals[i].GoalScore.Length) tempString += "\n";
						}
					}
					GUI.Box(new Rect(Screen.width*(i*.333333f), Screen.height*.17f, Screen.width*.13333333333f, Screen.height*.1f), 
					        "Best score:\n" + Goals[i].HighScore.ToString(), styleLibrary.ShopStyles.InGameGoalBox);
					GUI.Box(new Rect(Screen.width*(i*.333333f + .13333333333f), Screen.height*.17f, Screen.width*.1f, Screen.height*.1f), 
					        tempString, styleLibrary.ShopStyles.InGameGoalBox);
					
					if(GUI.Button(new Rect(Screen.width*(i*.333333f),0, Screen.width*.333333f, Screen.height*.27f), 
					              "", styleLibrary.ShopStyles.InGameGoalBoxHoverOverlay)) {
						GoalDisplay = new bool[] {false, false, false};
						clickBlocker.MoveToSpot(-1);
					}
				}
				else {
					GUI.Box(new Rect(Screen.width*(i*.333333f),Screen.height*.05f, Screen.width*.333333f, Screen.height*.075f), 
					        Goals[i].MiniDescription, styleLibrary.ShopStyles.InGameGoalBox);
					GUI.Box(new Rect(Screen.width*(i*.333333f + .11111f),0, Screen.width*.22222f, Screen.height*.05f), 
					        Goals[i].DisplayScore, styleLibrary.ShopStyles.InGameGoalBox);
				}
				if(GUI.Button(new Rect(Screen.width*(i*.333333f),0, Screen.width*.333333f, Screen.height*.125f), 
				              "", styleLibrary.ShopStyles.InGameGoalBoxHoverOverlay)){
					GoalDisplay = new bool[]{false, false, false};
					GoalDisplay[i] = true;
					clickBlocker.MoveToSpot(i);
				}
			}
		}
		#endregion
	}
}
