using UnityEngine;
using System.Collections;

public class GameControlGUI : MonoBehaviour {
    
	GameControl gameControl;
	ShopControlGUI shopControlGUI;

	string DisplayName;
	string DisplayRules;
	Texture2D DisplayRangeTexture;
//	float DisplayRangeSize;
	Texture2D DisplayAOETexture;
	float DisplayAOESize;
	Sprite DisplayCard;
	Texture2D DisplayCardIcon;
	Texture2D DisplayRarity;
	Texture2D DisplayIcon;
	GUIStyle DisplayTitleStyle;
	GUIStyle DisplayTextStyle;
	bool showingDeck;
	
	bool displayDim = false;
	SpriteRenderer displayCardRenderer;
	DimAnimate dimmer;
	public bool CardDisplay = false;
	GameObject cardObjFromDeck;

	bool showingDiscard;
	Vector3 originalDiscardPlacement = new Vector3(3f, 2f, 0);
	Vector3 displayDiscardPlacement = new Vector3(3f, 6.5f, 0);

	GUIStyleLibrary styleLibrary;

	void Start() {
		MonoBehaviour.useGUILayout = false;
		gameControl = gameObject.GetComponent<GameControl> ();
		styleLibrary = gameObject.GetComponent<GUIStyleLibrary> ();
		shopControlGUI = gameObject.GetComponent<ShopControlGUI> ();
		displayCardRenderer = GameObject.Find ("Display card").GetComponent<SpriteRenderer> ();
		dimmer = GameObject.Find ("Dimmer").GetComponent<DimAnimate>();
	}

	void Update() {
		if (displayDim && !Input.GetMouseButton (0)) {
			displayDim = false;
			Dim (false);
		}
	}

	void OnGUI () {
		
		if (showingDeck) {
			string tempString = "Cards left in \nthe deck:\n";
			GUI.Box(new Rect(0, 0, Screen.width*.2f, Screen.height*(gameControl.Deck.Count+3)*.03f), tempString);
			for(int i = 0; i < gameControl.Deck.Count; i++) {
				tempString += ((i+1) + ". " + gameControl.Deck[i]);
				while(GUI.Button(new Rect(Screen.width*.01f, Screen.height*.03f*(i+2), Screen.width*.18f, Screen.height*.028f), 
				                 gameControl.Deck[i])){
					string stringCardToDraw = "prefabs/cards/" + gameControl.Deck[i] + " card";
					cardObjFromDeck = (GameObject)GameObject.Instantiate (Resources.Load (stringCardToDraw));
					Card tempCard = cardObjFromDeck.GetComponent<Card>();
					Display(tempCard);
				}
			}
		}
		
		if (CardDisplay) {
			GUI.BeginGroup (new Rect (Screen.width*.2f, Screen.height*.165f, Screen.width*.6f, Screen.height*.55f), "");
			GUI.Box (new Rect (0,0, Screen.width*.33f, Screen.height*.2f), DisplayName, DisplayTitleStyle);
			GUI.Box (new Rect (Screen.width*.33f, 0, Screen.width*.25f, Screen.width*.25f), DisplayCardIcon, DisplayTitleStyle);
			GUI.Box(new Rect(0, Screen.height*.25f, Screen.width*.5f, Screen.height*.4f), DisplayRules, DisplayTextStyle); 
			if(DisplayRangeTexture != null) {
//				GUI.DrawTexture(new Rect(Screen.width*.05f, Screen.height*.38f, Screen.width*.1f*DisplayRangeSize, Screen.width*.1f*DisplayRangeSize), 
				GUI.DrawTexture(new Rect(Screen.width*.15f, Screen.height*.47f, Screen.width*.08f, Screen.width*.08f), 
			                                                DisplayRangeTexture);
			}
			if(DisplayAOETexture != null) {
//				GUI.DrawTexture(new Rect(Screen.width*.25f, Screen.height*.38f, Screen.width*.1f*DisplayAOESize, Screen.width*.1f*DisplayAOESize), 
				GUI.DrawTexture(new Rect(Screen.width*.25f, Screen.height*.47f, Screen.width*.08f, Screen.width*.08f), 
			                                              DisplayAOETexture);
			}
			GUI.DrawTexture(new Rect(Screen.width*.0f, Screen.height*.47f, Screen.width*.08f, Screen.width*.08f), DisplayRarity);
			GUI.DrawTexture(new Rect(Screen.width*.45f, Screen.height*.47f, Screen.width*.08f, Screen.width*.08f), DisplayIcon);
			GUI.EndGroup();
		}
		
		if (gameControl.Tooltip != "") {
			GUI.Box(new Rect(Screen.width*.02f, Screen.height*.68f, Screen.width*.8f, Screen.height*.08f), 
			        gameControl.Tooltip, styleLibrary.GameControlGUIStyles.TooltipBox);
		}
	}

	public void Display(Card card) {
		CardDisplay = true;
		if (Tutorial.TutorialLevel != 0) return;
		shopControlGUI.UnclickGoals ();
		displayCardRenderer.enabled = true;
		DisplayName = card.CardName;
		DisplayName.Replace ("\n", "");
		DisplayCardIcon = Resources.Load (card.IconPath) as Texture2D;
		DisplayRules = card.DisplayText;
		if (card.maxRange != 0) {
			DisplayRangeTexture = (Texture2D)Resources.Load("sprites/targeting icons/range " + card.rangeTargetType.ToString() + 
			                                                " " + card.minRange.ToString() + "-" + card.maxRange.ToString());
//			DisplayRangeSize = (card.maxRange*2+1)*.2f;
		}
		else 
			DisplayRangeTexture = null;
		
		if(card.aoeMaxRange != 0) {
			DisplayAOETexture = (Texture2D)Resources.Load("sprites/targeting icons/aoe " + card.aoeTargetType.ToString() + 
			                                              " " + card.aoeMinRange.ToString() + "-" + card.aoeMaxRange.ToString());
//			DisplayAOESize = card.aoeMaxRange;
		}
		else {
			DisplayAOETexture = null;
		}
		
		DisplayTitleStyle = new GUIStyle(styleLibrary.GameControlGUIStyles.DisplayTitle);
		DisplayTitleStyle.fontSize = card.cardUI.DisplayTitleFontSize;
		DisplayTextStyle = new GUIStyle(styleLibrary.GameControlGUIStyles.DisplayText);
		DisplayTextStyle.fontSize = card.cardUI.DisplayRulesFontSize;
		
		//default text color is black
		DisplayTitleStyle.normal.textColor = new Color(0,0,0);
		DisplayTextStyle.normal.textColor = new Color(0,0,0);
		
		int godnum = ShopControl.AllGods.IndexOf (card.God);
		
		DisplayCard = shopControlGUI.GodDisplayCards [godnum];
		DisplayIcon = shopControlGUI.GodIcons [godnum];
		
		if(card.God == ShopControl.Gods.Ekcha | card.God == ShopControl.Gods.Ixchel) {
			DisplayTitleStyle.normal.textColor = new Color(1,1,1);
			DisplayTextStyle.normal.textColor = new Color(1,1,1);
		}
		
		switch(card.ThisRarity) {
		case (Card.Rarity.Paper):
			DisplayRarity = shopControlGUI.PaperTexture;
			break;
		case (Card.Rarity.Bronze):
			DisplayRarity = shopControlGUI.BronzeTexture;
			break;
		case (Card.Rarity.Silver):
			DisplayRarity = shopControlGUI.SilverTexture;
			break;
		case (Card.Rarity.Gold):
			DisplayRarity = shopControlGUI.GoldTexture;
			break;
		}
		displayCardRenderer.sprite = DisplayCard;
		
		CardDisplay = true;
	}
	
	public void Undisplay() {
		CardDisplay = false;
		//this vvv might be a bad condition to base whether or not to turn off the dimmer on. it works for now though.
		if(gameControl.CardsToTarget == 0) {
			Dim(false);
		}
		displayCardRenderer.enabled = false;
		if(cardObjFromDeck != null) Destroy(cardObjFromDeck);
	}

	public void DisplayDim() {
		displayDim = true;
		Dim ();
	}

	public void Dim()
	{
		Dim(true);
	}

	public void Dim(bool TurnOn)
	{
		if (TurnOn)
		{
			dimmer.Dim();
		}
		else
		{
			dimmer.Undim();
		}
	}

	public void ForceDim() {
		dimmer.ForceDim ();
	}

	public void UnlockDim() {
		dimmer.UnlockDim ();
	}

	public void ShowDeck(bool TurningOn) {
		return;
		showingDeck = TurningOn;
	}

	public void SetDiscardPilePosition ()
	{
		GameObject discard = GameObject.FindGameObjectWithTag ("Discard");
		discard.transform.localPosition = originalDiscardPlacement;
	}

	public void FlipDiscard() {
		GameObject discard = GameObject.FindGameObjectWithTag ("Discard");
		
		if(!showingDiscard) {
			discard.transform.localPosition = displayDiscardPlacement;
			
			for(int i = 0; i < gameControl.Discard.Count; i++) {
				CardUI tempcardui = gameControl.Discard[i].GetComponent<CardUI>();
				tempcardui.MoveAnimateWhileDiscarded(i, true);
			}
		}
		else {
			discard.transform.localPosition = originalDiscardPlacement;
		}
		showingDiscard = !showingDiscard;
	}
}
