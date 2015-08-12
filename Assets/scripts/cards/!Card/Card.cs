﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {

	public Sprite tempSprite;

	//VERY IMPORTANT BOOL
	public bool Discarded;
	public bool Targeted;
	public bool Selected;
	public bool Peeked;

	public GameControl gameControl;
	public GridControl gridControl;
	public ShopControl shopControl;
	public ClickControl clickControl;
	public OptionControl optionControl;
	public EventGUI eventGUIControl;
	public GameControlGUI GameControlGUI;
	public GUIStyleLibrary styleLibrary;

	public CardLibrary library;
	public GameObject hand;
	public GameObject discardPileObj;
	public GameObject playerObj;

	public int CardsToTarget = 0;
	public bool CardsToTargetWillBeDiscarded = false;
	public bool CardsToTargetWillBePeeked = false;

	//Animation variables
	public bool Animating = false;
	public bool DrawAnimating = false;
	public bool DiscardAnimating = false;
	public bool BurnAnimating = false; 
	public bool ShuffleAnimating = false;
	public Transform DeckTransform;
	public GameObject CardBackObject;
	public Sprite CardBackSprite;
	public float DrawStartTime;
	public float MoveUpStartTime;
	public Vector3 StartPosition;
	public Vector3 HighestPoint;
	public bool BehindPlayBoard = false;
	public Vector3 DrawEndPosition;
	public Vector3 DiscardEndPosition;
	public enum CardActionTypes { Armor, NoTarget, NoTargetNoInput, NoTargetGridSquare, TargetGridSquare, TargetCard, Options };
	public enum Rarity { Paper, Bronze, Silver, Gold, Platinum };
	public ButtonAnimate PlayButton;
	public SpriteRenderer[] SRenderers;
	public MeshRenderer[] meshrenderers;
	public int TitleFontSize = 45;
	public int SmallFontSize = 45;

	//Card-specific variables, to be set in the Start() function before calling base.Start()
	public string CardName;
	public string DisplayName;
	public string PrefabPath;
	public string Tooltip;
	public string DisplayText;
	public string MiniDisplayText;
	public string IconPath;
	public GridControl.TargetTypes rangeTargetType;
	public int minRange;
	public int maxRange;
	public GridControl.TargetTypes aoeTargetType;
	public int aoeMinRange;
	public int aoeMaxRange;
	public Rarity ThisRarity;
	public CardActionTypes CardAction;
	public ShopControl.Gods God;

	public bool DiscardWhenPlayed = true;
	public bool BurnsSelfWhenPlayed = false;
	public int Cost;

	public bool ForcingDiscardOfThis = false;
	public bool FreeTargetSquare = false;
	
	//armor values. would work better if i made armors inherit from a base class Armor, but too bad
	public int DamageProtection;
	public int DamageDistanceProtectionMax;
	public int DamageDistanceProtectionMin;
	public bool ArmorHasBeenUsed;
	public bool ArmorWithSecondaryAbility = false;

	public bool TriggerResetsOnNewTurn = true;

	public string TriggerMessage = "";

	public ShineAnimation ShineAnim;
    public SpriteRenderer Glow;

	//////////////////////////////////////
	/// INITIALIZE METHOD -- CALLED BY PEEK() AND DRAW()
	//////////////////////////////////////

	public virtual void Initialize() {
		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		gameControl = gameController.GetComponent<GameControl>();
		gridControl = gameController.GetComponent<GridControl>();
		shopControl = gameController.GetComponent<ShopControl>();
		clickControl = gameController.GetComponent<ClickControl>();
		optionControl = gameController.GetComponent<OptionControl>();
		library =  gameController.GetComponent<CardLibrary>();
		GameControlGUI = gameController.GetComponent<GameControlGUI> ();
		PlayButton = GameObject.Find("play end button").GetComponent<ButtonAnimate>();
		hand = GameObject.Find("Hand");
		discardPileObj = GameObject.Find("Discard pile");
		playerObj = GameObject.FindGameObjectWithTag("Player");
		meshrenderers = GetComponentsInChildren<MeshRenderer>();
		SRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();

		LibraryCard tempLibraryCard = CardLibrary.Lib[CardName];
		name = CardName;

		CardName = tempLibraryCard.CardName;
		DisplayName = tempLibraryCard.DisplayName;
		IconPath = "sprites/card icons/" + tempLibraryCard.IconPath;
		PrefabPath = tempLibraryCard.PrefabPath;
		Tooltip = tempLibraryCard.Tooltip;
		DisplayText = tempLibraryCard.DisplayText;
		MiniDisplayText = tempLibraryCard.MiniDisplayText;
		rangeTargetType = tempLibraryCard.RangeTargetType;
		minRange = tempLibraryCard.rangeMin;
		maxRange = tempLibraryCard.rangeMax;
		aoeTargetType = tempLibraryCard.AoeTargetType;
		aoeMinRange = tempLibraryCard.aoeMinRange;
		aoeMaxRange = tempLibraryCard.aoeMaxRange;
		ThisRarity = tempLibraryCard.ThisRarity;
		CardAction = tempLibraryCard.CardAction;
		God = tempLibraryCard.God;

		CardBackSprite = Resources.Load("sprites/cards/real pixel card back") as Sprite;
			
		gameControl.Hand.Add(gameObject);

		TextMesh[] textMeshes = gameObject.GetComponentsInChildren<TextMesh>();
		foreach(TextMesh text in textMeshes) {
			if(text.gameObject.name == "name text") {
				text.fontSize = TitleFontSize;
				text.text = DisplayName;
			}
			else {
				text.fontSize = SmallFontSize;
				text.text = MiniDisplayText;
			}

			if(God == ShopControl.Gods.Ekcha | God == ShopControl.Gods.Ixchel) {
				text.color = new Color(1, 1, 1);
			}
		}
		
		CardText[] cardTexts = gameObject.GetComponentsInChildren<CardText>();
		SpriteRenderer[] renders = gameObject.GetComponentsInChildren<SpriteRenderer>();	
		foreach(SpriteRenderer render in renders) {
			if (render.gameObject.tag == "cardback") { continue; }
			else if (render.gameObject.name == "picture")
			{
				if (IconPath != "")
				{
					tempSprite = Resources.Load<Sprite>(IconPath);
					render.sprite = Resources.Load<Sprite>(IconPath);
				}
			}
			else if (render.gameObject.name == "rarity")
			{
				switch (tempLibraryCard.ThisRarity)
				{
					case Card.Rarity.Gold:
						render.sprite = shopControl.Gold;
						Cost = 10;
						break;
					case Card.Rarity.Silver:
						render.sprite = shopControl.Silver;
						Cost = 6;
						break;
					case Card.Rarity.Bronze:
						render.sprite = shopControl.Bronze;
						Cost = 3;
						break;
					case Card.Rarity.Paper:
						render.sprite = shopControl.Paper;
						Cost = 0;
						break;
				}
			}
			else if (render.gameObject.name == "god icon")
			{
				int godnum = ShopControl.AllGods.IndexOf(tempLibraryCard.God);

				render.sprite = shopControl.SpriteGodIcons[godnum];
			}
			else if (render.gameObject.name == "shine animation")
			{
				ShineAnim = render.gameObject.GetComponent<ShineAnimation>();
			}
			else if (render.gameObject.name == "shine animation 2") { continue; }
            else if (render.gameObject.name == "glow")
            {
                Glow = render.gameObject.GetComponent<SpriteRenderer>();
            }
            else if (render.gameObject.name != "picture")
            {
                int godnum = 3;
                godnum = ShopControl.AllGods.IndexOf(tempLibraryCard.God);

                render.sprite = shopControl.GodSmallCards[godnum];
            }
		}
        foreach (CardText cardText in cardTexts) cardText.Initialize(101 - HandIndex() * 2);
    }
	
	//////////////////////////////////////
	/// Card specific tooltip
	//////////////////////////////////////

	public virtual void OnGUI() {
		if(Selected && Tooltip != "" && gameControl.Tooltip == ""){
			GUI.Box(new Rect(Screen.width * .02f, Screen.height * .68f, Screen.width * .8f, Screen.height * .08f), 
			        Tooltip, styleLibrary.CardStyles.Tooltip);
		}
		if(gameControl.CardsToTarget != 0 && Tooltip == "") {
			GUI.Box(new Rect(Screen.width*.02f, Screen.height*.72f, Screen.width*.8f, Screen.height*.04f), 
			        "Please select " + gameControl.CardsToTarget.ToString() + " cards", styleLibrary.CardStyles.Tooltip);
		}
	}

	//Update: just for animations
	public virtual void Update() {
		if(ShuffleAnimating) {
			if(Time.time > DrawStartTime + .48f) {
				Animating = false;
				DrawAnimating = false;	
				ShuffleAnimating = false;

				gameObject.GetComponent<SpriteRenderer>().enabled = false;
				return;
			}
			else {
				float time = Time.time - DrawStartTime;
				transform.position = Vector3.Lerp(transform.position, DeckTransform.position, time);
			}
		}
		else if(DiscardAnimating) {
			if(MoveUpStartTime + .25f > Time.time) {
				float time =((Time.time-MoveUpStartTime));
				transform.localPosition = Vector3.Lerp(transform.localPosition, HighestPoint, time*4);
				
				return;
			}
			else if(( MoveUpStartTime + .5f > Time.time ) &&(Time.time > MoveUpStartTime + .25f)) {  
				if(!BehindPlayBoard &&(DiscardWhenPlayed | ForcingDiscardOfThis)){ 
					SpriteRenderer[] spriterenderererers = GetComponentsInChildren<SpriteRenderer>();
					foreach(SpriteRenderer sr in spriterenderererers) {
                        sr.sortingLayerName = "Field background";
                        if (sr.gameObject.name != "rarity" && sr.gameObject.name != "glow" && sr.gameObject.name != "god icon" && sr.gameObject.name != "picture")
                        {
                            sr.sortingOrder = 0;
                        }
                        else
                        {
                            sr.sortingOrder = 1;
                        }
					}
					MeshRenderer[] meshrenderers = GetComponentsInChildren<MeshRenderer>();
					foreach(MeshRenderer meshrenderer in meshrenderers){
                        meshrenderer.sortingLayerName = "Field background";
						meshrenderer.sortingOrder = 1;
					}
					
					BehindPlayBoard = true;
					StartPosition = new Vector3(StartPosition.x, StartPosition.y-1f, StartPosition.z);
					return;
				}
				float time =((Time.time-MoveUpStartTime - .25f));
				transform.localPosition = Vector3.Lerp(transform.localPosition, StartPosition, time*4);
				return;
			}
			else if( Time.time > MoveUpStartTime + .5f ) {
				ForcingDiscardOfThis = false;
				FinishDiscard();
				return;
			}
		}
		else if(DrawAnimating) {
			if(CardBackObject != null) {
				gameObject.transform.localScale = Vector3.Lerp(new Vector3(gameObject.transform.localScale.x, .85f, 1f), new Vector3(0, .85f, 1f), Time.time-DrawStartTime);

				if(Time.time >= DrawStartTime + .25f) {
					Destroy(CardBackObject);
				}
			}
			else {
				gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(.82f, .85f, .8f), Time.time-DrawStartTime);
			}

			if(Time.time > DrawStartTime + .48f) {
				//this ends the animation
				Animating = false;
				DrawAnimating = false;	
				transform.localScale = new Vector3(.82f, .85f, .8f);
				
				transform.localPosition = DrawEndPosition;
				return;
			}
			else {
				float time = Time.time - DrawStartTime;
				
				transform.localPosition = Vector3.Lerp(transform.localPosition, DrawEndPosition, time);
			}
		}
		else if(BurnAnimating) {
			float fade = Time.time - DrawStartTime;
			foreach(SpriteRenderer SRenderer in SRenderers) {
				SRenderer.color = new Color(1f, 1f, 1f, 1-fade*2);
			}
			if(DrawStartTime + .5f < Time.time) {
				DestroyThisGameObject();
			}
		}
		else if(Animating) {

			if(Time.time > DrawStartTime + .48f) {
				//this ends the animation
				Animating = false;
				DrawAnimating = false;	
				ShuffleAnimating = false;
				transform.localPosition = DrawEndPosition;
				if(!Discarded) {
					transform.localScale = new Vector3(.82f, .85f, 0);
				}
				
				return;
			}
			else if(Time.time > DrawStartTime + .25f) {
				//midpoint of animation
				float time = Time.time - DrawStartTime;
				transform.localPosition = Vector3.Lerp(transform.localPosition, DrawEndPosition, time);
			}
			else {
				float time = Time.time - DrawStartTime;
				transform.localPosition = Vector3.Lerp(transform.localPosition, DrawEndPosition, time);
			}
		}
	}

	//////////////////////////////////////
	/// Universal card methods that incorporate animations as well as game logic
	//////////////////////////////////////

	//select, deselect
	public void Select() {
		gameControl.DeselectCards();
		gridControl.DestroyAllTargetSquares();
		GameControlGUI.Dim(false);

		if(gameControl.PlaysLeft > 0) { 
			if(gameObject != null) {
				transform.Translate(new Vector3(0f, .25f, 0f));
				Selected = true;
			}
		}
		else {
			if(CardAction != CardActionTypes.Armor && CardAction != CardActionTypes.TargetGridSquare) {
				PlayButton.ErrorAnimation();
			}
		}
	}
	public void Deselect() {
		if(Selected) transform.Translate(new Vector3(0f, -.25f, 0f));
		Selected = false;
	}
	//target, untarget
	public void Target() {
		transform.Translate(new Vector3(0f, .25f, 0f));
		Targeted = true;
	}
	public void Untarget() {
		transform.Translate(new Vector3(0f, -.25f, 0f));
		Targeted = false;
	}
	//draw 
	public void DrawAnimate(int position) {
		gameObject.transform.parent = gameControl.handObj.transform;
		gameObject.transform.localPosition = new Vector3(-2.7f, .5f, 0);

		DrawAnimating = true;
		CardBackObject =(GameObject)GameObject.Instantiate(Resources.Load("prefabs/Drawn card prefab"));
		CardBackObject.transform.parent = gameObject.transform;
		CardBackObject.transform.localPosition = new Vector3(0, 0, 0);
		CardBackObject.transform.localScale = new Vector3(2.9f, 2.8f, 2f);
		SpriteRenderer CardBackSR = CardBackObject.GetComponent<SpriteRenderer>();
		CardBackSR.sortingLayerID = 0;
		CardBackSR.sortingOrder = 4;

		MoveAnimate(position);
	}

	public void MoveAnimate(int position) {

		int index = HandIndex();
		
		foreach(MeshRenderer meshrenderer in meshrenderers){
			meshrenderer.sortingLayerName = "Card";
			meshrenderer.sortingOrder = 101-index*2;
		}

		foreach(SpriteRenderer SRenderer in SRenderers) {
			if(SRenderer.gameObject.name == "rarity" | SRenderer.gameObject.name == "god icon" | SRenderer.gameObject.name == "picture") {
				SRenderer.sortingOrder = 101-index*2;
			}
			//this catches the card background VVV
            else if (SRenderer.gameObject.name == "glow" | SRenderer.gameObject.name == "shine animation")
			{
				//it's already at sorting order 3000. don't do anything
			}
			else {
				SRenderer.sortingOrder = 100-index*2;
			}
		}
		DrawEndPosition = new Vector3((position)*1.55f - 1f, .1f, 0);
		Animating = true;
		DrawStartTime = Time.time;
	}
	public void MoveAnimateWhileDiscarded(int position, bool expand ) {
		if(expand) {
			DrawEndPosition = new Vector3(0f,(position) * -.2f + -.5f, 0);
		} else {
			DrawEndPosition = new Vector3(0f,(position) * -.5f + -.5f, 0);
		}
		Animating = true;
		DrawStartTime = Time.time;
	}
	public void ShuffleMoveAnimate(Transform Deck) {
		DeckTransform = Deck;
		transform.parent = gameControl.handObj.transform;
//		Animating = true;
		DrawStartTime = Time.time;
		ShuffleAnimating = true;

	}

	public void DiscardAnimate() {
		MoveUpStartTime = Time.time;
		DiscardAnimating = true;
		StartPosition = transform.localPosition;
		HighestPoint = StartPosition;
		HighestPoint.y = StartPosition.y + 2f;

		if(DiscardWhenPlayed | ForcingDiscardOfThis) {
			Discarded = true;
			gameControl.Hand.Remove(gameObject);
			gameControl.Discard.Add(gameObject);
		}
	}
	public void FinishDiscard() {
		DiscardAnimating = false;
		Deselect();
		if(DiscardWhenPlayed | ForcingDiscardOfThis) {
	//		Discarded = true;
	//		gameControl.Hand.Remove(gameObject);
	//		gameControl.Discard.Add(gameObject);
			Animating = false;
			BurnAnimating = false;
			DrawAnimating = false;
			ShuffleAnimating = false;

			int index = gameControl.Discard.IndexOf(gameObject);

			transform.parent = discardPileObj.transform;
			transform.localPosition = new Vector3(0,(index+1) * -.2f, 0);

			SpriteRenderer[] SRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
			foreach(SpriteRenderer SRenderer in SRenderers) {
				if(SRenderer.gameObject.name == "rarity" | SRenderer.gameObject.name == "glow" | SRenderer.gameObject.name == "god icon" | SRenderer.gameObject.name == "picture") {
                    SRenderer.sortingLayerName = "Card";
					SRenderer.sortingOrder = 101+index*2;
				}
				else {
                    SRenderer.sortingLayerName = "Card";
					SRenderer.sortingOrder = 100+index*2;
				}
			}
			MeshRenderer[] meshes = gameObject.GetComponentsInChildren<MeshRenderer>();
			foreach(MeshRenderer mesh in meshes) {
                mesh.sortingLayerName = "Card";
				mesh.sortingOrder = 101+index*2;
			}
		}

		ForcingDiscardOfThis = false;
		
		shopControl.GoalCheck("Discard pile has X cards in it");
		shopControl.GoalCheck("Discard pile has X cards in a row with the same God");

		//	i put OrganizeCards into CheckQ
		//	Invoke("OrganizeCards", .3f);
	}

	public virtual void Tuck() {
		Debug.Log("Placing this card back into the deck");

		clickControl.DisallowEveryInput();
		DrawEndPosition = new Vector3(-2.7f, 3f, 0);
		Animating = true;
		DrawStartTime = Time.time;

		if(gameControl.Hand.Contains(gameObject)) 
			gameControl.Hand.Remove(gameObject);
		if (gameControl.Discard.Contains (gameObject)) 
			gameControl.Discard.Remove(gameObject);
		if(gameControl.PeekedCards.Contains(gameObject)) 
			gameControl.PeekedCards.Remove(gameObject);

		Invoke("FinishTuck", .25f);
	}
	public void FinishTuck(){
		string tempString = CardName;
		tempString.Replace("\n", " ");
		gameControl.Deck.Add(tempString);
		Destroy(gameObject);

		clickControl.AllowEveryInput();
	}

	public virtual void Burn() {
		EventControl.EventCheck("Burn");
		clickControl.DisallowEveryInput();
		if(Selected)
			Deselect();
		gameControl.Hand.Remove(gameObject);
		DrawStartTime = Time.time;
		BurnAnimating = true;
	}

	public void Peek(int position, int maxPositions) {
		Peeked = true;

		foreach(MeshRenderer meshrenderer in meshrenderers){
			meshrenderer.sortingLayerName = "Card";
			meshrenderer.sortingOrder = 101-position*2;
		}
		
		foreach(SpriteRenderer SRenderer in SRenderers) {
			if(SRenderer.gameObject.name == "rarity" | SRenderer.gameObject.name == "glow" | SRenderer.gameObject.name == "god icon" | SRenderer.gameObject.name == "picture") {
				SRenderer.sortingOrder = 101-position*2;
			}
			//this catches the card background VVV
			else {
				SRenderer.sortingOrder = 100-position*2;
			}
		}

		transform.localPosition = new Vector3(position + -1f, 1f, 0);
	}

	public virtual void PeekCallback() {
		Debug.Log ("peeked!");
	}

	//////////////////////////////////////
	/// METHODS USED FOR EXECUTING THE CARD--THESE ARE UNIVERSAL
	//////////////////////////////////////
	
	public virtual void Click() {

		GameControlGUI.Dim(false);

		if(Discarded) {
			return;
		}		

		if(CardAction == CardActionTypes.TargetGridSquare) {
			Activate();
			return;
		}

		if(Selected) {
			//Extreme corner case, this prevents Target Card cards from being played without valid targets
			if(CardAction == CardActionTypes.TargetCard && ((CardsToTargetWillBeDiscarded && gameControl.Discard.Count < 1) | 
															(!CardsToTargetWillBeDiscarded && gameControl.Hand.Count < 2)    ) ) {
				Debug.Log("yeah!!!!");
				gameControl.Tooltip = "You can't play this card right now, because it can't target a card.";
				return;
			}

			Activate(false);
		}
		else {
			Select();
		}
	}

	//VV this is almost never going to be overwritten, except in cases like DragonWhiskey where the card 
	//has multiple steps so it runs CheckQ() at the end instead of halfway through.
	public virtual void Activate() { Activate(false); }
	public virtual void Activate(bool FreePlay) {
		if(!FreePlay && CardAction != CardActionTypes.TargetGridSquare)
			gameControl.AddPlays(-1);

		clickControl.DisallowEveryInput();

		switch(CardAction) {
		case CardActionTypes.Armor:
			break;
		case CardActionTypes.NoTarget:
			Play();
			DiscardOrBurnIfNotInQ();
			clickControl.AllowInputUmbrella = false;
			CheckQ();
			break;
		case CardActionTypes.NoTargetNoInput:
			Play();
			DiscardOrBurnIfNotInQ();
			break;
		case CardActionTypes.NoTargetGridSquare:
			Play();
			DiscardOrBurnIfNotInQ();
			clickControl.AllowInputUmbrella = false;
			gridControl.MakeSquares(aoeTargetType, aoeMinRange, aoeMaxRange, false);
			CheckQ();
			break;
		case CardActionTypes.TargetGridSquare:
			EnterTargetingMode();
			clickControl.AllowEveryInput();
			break;
		case CardActionTypes.TargetCard:
			if((CardsToTargetWillBePeeked && gameControl.PeekedCards.Count < CardsToTarget) |
			   (CardsToTargetWillBeDiscarded && gameControl.Discard.Count < CardsToTarget) |
			   (!CardsToTargetWillBeDiscarded && gameControl.Hand.Count < CardsToTarget)) {
				Tooltip = "Not enough cards to target!";
				DiscardOrBurnIfNotInQ();
				gameControl.AnimateCardsToCorrectPosition();
				return;
			}

			//VVV by default, the cards to target are not discarded or peeked, but you can change that in the Play()
			clickControl.AllowInputUmbrella = false;
			ReallowUmbrellaInputAfterDiscardOrBurn();

			clickControl.DisallowEveryInput();
			clickControl.AllowCardTargetInput = true;
			clickControl.AllowInfoInput = true;

			if(CardsToTargetWillBeDiscarded) 
				gameControl.CardsToTargetAreDiscarded = true;
			else 
				gameControl.CardsToTargetAreDiscarded = false;

			if(CardsToTargetWillBePeeked) 
				gameControl.CardsToTargetArePeeked = true;
			else
				gameControl.CardsToTargetArePeeked = false;

			gameControl.TargetCardCallback = this;
			
			Play();
			DiscardOrBurnIfNotInQ();
			gameControl.AnimateCardsToCorrectPosition();
			break;
		case CardActionTypes.Options:
			Play();
			DiscardOrBurnIfNotInQ();
			clickControl.DisallowEveryInput();
			GameControlGUI.Dim();
			break;
		default:
			Debug.Log("FUCK");
			break;
		}
	}
	public void EnterTargetingMode() {
		Select();
		gridControl.EnterTargetingMode(rangeTargetType, minRange, maxRange);
		gameControl.TargetSquareCallback = this;
		gameControl.Tooltip = "Please select a square.";
		
		clickControl.AllowInputUmbrella = true;
		clickControl.AllowSquareTargetInput = true;
		clickControl.AllowInfoInput = true;
	}

	public virtual void Play() {
		shopControl.GoalCheck("Play X cards in one turn");
		
		if(ThisRarity == Rarity.Paper) {
			shopControl.GoalCheck("Play X paper cards in one turn");
		}
		
		shopControl.GoalCheck("Play less than X cards total");
		shopControl.GoalCheck("Don't play a card X turns in a row");
	}
	public virtual void ArmorPlay() {
		DiscardOrBurnIfNotInQ();
		Play();
	}
	public void DiscardOrBurnIfNotInQ(){
		if(!QControl.QContains(this)) {
			if(BurnsSelfWhenPlayed)
				Burn();
		}

		if(!BurnsSelfWhenPlayed && !Discarded && !DiscardAnimating) {
				DiscardAnimate();
		}
	}

	public void ReallowUmbrellaInputAfterDiscardOrBurn() {
		clickControl.Invoke("ChangeUmbrellaInputAllowToTrue", .53f);
	}

	///
	/// Q checking methods
	/// 

	public void CheckQ() {
		Invoke("ActuallyCheckQ", .3f);
	}
	public void ActuallyCheckQ() {
		QControl.CheckQ();
	}
	
	/// 
	/// Callback methods
	/// 

	public virtual void AfterCardTargetingCallback() {
		gameControl.TargetedCards = new List<GameObject>();
		gameControl.CardsToTarget = 0;
		GameControlGUI.Dim(false);

	//	i put OrganizeCards into CheckQ
	//	Invoke("OrganizeCards", .3f);
		CheckQ();
	}
	//GiveOptions method. First you Play(), which sends options to OptionControl, then you pick an option, then it calls this method
	public virtual void OptionsCalledThis(bool ResponseIsYes) {

		optionControl.TurnOffOptions();

		//	i put OrganizeCards into CheckQ
		//	Invoke("OrganizeCards", .3f);
		clickControl.AllowEveryInput();
		CheckQ();
	}
	public virtual void OptionsCalledThis(int Choice) {

		optionControl.TurnOffOptions();
		
		//	i put OrganizeCards into CheckQ
		//	Invoke("OrganizeCards", .3f);

		clickControl.AllowEveryInput();
		
		CheckQ();
	}
	//TargetGridSquare methods. First, a bunch of targetsquares are spawned, then when one is clicked on it calls TargetSquareCalledThis which calls FindAndAffectUnits
	//which Affects each unit in the area. Simple battle spells use this.
	public virtual void TargetSquareCalledThis(int x, int y) {
		if(!FreeTargetSquare) {
			DiscardOrBurnIfNotInQ();
			
			gameControl.AddPlays(-1);

			clickControl.DisallowEveryInput();
		}

		gridControl.DestroyAllTargetSquares();
	
		FindAndAffectUnits(x, y);
		gridControl.MakeSquares(aoeTargetType, aoeMinRange, aoeMaxRange, x, y, false);

	//	Play();
		
		CheckQ();
	}
	public virtual void EventCall(string EventName) { 	}

	/// <summary>
	/// Finds the and affect units.
	/// </summary>
	/// <param name="clickedX">Clicked x.</param>
	/// <param name="clickedY">Clicked y.</param>
	public void FindAndAffectUnits(int clickedX, int clickedY){

		gridControl.FindAllGridUnits();
			
		for(int i = gridControl.gridUnits.Count-1; i > -1; i--) {
			if(aoeTargetType == GridControl.TargetTypes.diamond) {
				int distance =(Mathf.Abs(clickedX - gridControl.gridUnits[i].xPosition) + Mathf.Abs(clickedY - gridControl.gridUnits[i].yPosition));
				if(distance >= aoeMinRange && distance <= aoeMaxRange){
					Affect(gridControl.gridUnits[i]);
				}
			}
			else if(aoeTargetType == GridControl.TargetTypes.none) {
				if(clickedX == gridControl.gridUnits[i].xPosition && clickedY == gridControl.gridUnits[i].yPosition) {
					Affect(gridControl.gridUnits[i]);
				}
			}
			else if(aoeTargetType == GridControl.TargetTypes.square) {
				int xDifference = Mathf.Abs(clickedX - gridControl.gridUnits[i].xPosition);
				int yDifference = Mathf.Abs(clickedY - gridControl.gridUnits[i].yPosition);
				if(xDifference >= aoeMinRange && xDifference <= aoeMaxRange && 
				   yDifference >= aoeMinRange && yDifference <= aoeMaxRange) {
					Affect(gridControl.gridUnits[i]);
				}
			}
			else 
				Debug.Log("the card should have a rangeTargetType!");
		}
	}
	public virtual void Affect(GridUnit x) { Debug.Log("Card parentclass's Affect() was called. Bad!"); }

	//////////////////////////////////////
	/// QCall methods
	//////////////////////////////////////

	public virtual void OptionQCall() {
		Debug.Log("Default OptionQCall method called!");
	}
	public virtual void SpecialQCall() {
		Debug.Log("Default SpecialQCall method called!");
	}

	//////////////////////////////////////
	/// UTILITIES--THESE ARE KEYWORDS FOR COMPLEX EFFECTS THAT MANY CARDS WILL HAVE
	//////////////////////////////////////

	public void BasicDamageEffect(GridUnit gridUnit, int damageTaken) {
		if(gridUnit.gameObject.tag == "Player") {
			Player player = gridUnit.gameObject.GetComponent<Player>();
			player.TakeDamage(damageTaken, 0);
		}
		if(gridUnit.gameObject.tag == "Enemy") {
			Enemy enemy = gridUnit.gameObject.GetComponent<Enemy>();
			enemy.TakeDamage(damageTaken);
		}
	}

	public void BasicStunEffect(GridUnit gridUnit) {
		if(gridUnit.gameObject.tag == "Player") {
			Player player = gridUnit.gameObject.GetComponent<Player>();
			player.StunnedForXTurns++;
		}
		if(gridUnit.gameObject.tag == "Enemy") {
			Enemy enemy = gridUnit.gameObject.GetComponent<Enemy>();
			enemy.StunnedForXTurns++;
		}
	}

	public int HandIndex() {
		if(gameControl == null) {
			gameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
		}
		for(int i = 0; i < gameControl.Hand.Count; i++) {
			if(gameControl.Hand[i] == gameObject) {
				return i;
			}
		}
		return 555;
	}

	void DestroyThisGameObject() {
		EventControl.RemoveFromLists(this);
		Destroy(gameObject);
	}

	public void ShineAnimate()
	{
		ShineAnim.Animate();
	}

    public void GlowAnimate(bool turnOn)
    {
        Glow.enabled = turnOn;
    }
}