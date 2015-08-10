using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {

	public Sprite tempSprite;

	//VERY IMPORTANT BOOL
	public bool Discarded;
	public bool Targeted;
	public bool Selected;
	public bool Peeked;

	public GameControl battleBoss;
	public GridControl gridBoss;
	public ShopControl shopBoss;
	public ClickControl clickBoss;
	public OptionControl optionBoss;
	public EventGUI eventGUIBoss;
	public GameControlUI gameControlUI;

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
	public enum Rarity { Paper, Copper, Silver, Gold, Platinum };
	public GUISkin gooeyskin;
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
		GameObject boss = GameObject.FindGameObjectWithTag("GameController");
		battleBoss = boss.GetComponent<GameControl>();
		gridBoss = boss.GetComponent<GridControl>();
		shopBoss = boss.GetComponent<ShopControl>();
		clickBoss = boss.GetComponent<ClickControl>();
		optionBoss = boss.GetComponent<OptionControl>();
		eventGUIBoss = boss.GetComponent<EventGUI>();
		library =  boss.GetComponent<CardLibrary>();
		PlayButton = GameObject.Find("play end button").GetComponent<ButtonAnimate>();
		hand = GameObject.Find("Hand");
		discardPileObj = GameObject.Find("Discard pile");
		playerObj = GameObject.FindGameObjectWithTag("Player");
		meshrenderers = GetComponentsInChildren<MeshRenderer>();
		SRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();

		gooeyskin =(GUISkin)Resources.Load("GUISkins/battleboss guiskin");

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
			
		battleBoss.Hand.Add(gameObject);

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
			if (render.gameObject.tag == "cardback")
			{

			}
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
				//this is for the stuff on the card. the card itself should be behind all this!

				switch (tempLibraryCard.ThisRarity)
				{
					case Card.Rarity.Gold:
						render.sprite = shopBoss.Gold;
						Cost = 10;
						break;
					case Card.Rarity.Silver:
						render.sprite = shopBoss.Silver;
						Cost = 6;
						break;
					case Card.Rarity.Copper:
						render.sprite = shopBoss.Copper;
						Cost = 3;
						break;
					case Card.Rarity.Paper:
						render.sprite = shopBoss.Paper;
						Cost = 0;
						break;
				}
			}
			else if (render.gameObject.name == "god icon")
			{
				//this is for the stuff on the card. the card itself should be behind all this!

				int godnum = ShopControl.AllGods.IndexOf(tempLibraryCard.God);

				render.sprite = shopBoss.SpriteGodIcons[godnum];
			}
			else if (render.gameObject.name == "shine animation")
			{
				ShineAnim = render.gameObject.GetComponent<ShineAnimation>();
			}
            else if (render.gameObject.name == "shine animation 2") { }
            else if (render.gameObject.name == "glow")
            {
                Glow = render.gameObject.GetComponent<SpriteRenderer>();
            }
            else if (render.gameObject.name != "picture")
            {
                //this is for the stuff on the card. the card itself should be behind all this!

                int godnum = 3;
                godnum = ShopControl.AllGods.IndexOf(tempLibraryCard.God);

                render.sprite = shopBoss.GodSmallCards[godnum];
            }
		}
        foreach (CardText cardText in cardTexts) cardText.Initialize(101 - HandIndex() * 2);
    }
	
	//////////////////////////////////////
	/// ONGUI
	//////////////////////////////////////

	public virtual void OnGUI() {
		if(Selected && Tooltip != "" && battleBoss.Tooltip == ""){
			GUI.Box(new Rect(Screen.width * .02f, Screen.height * .68f, Screen.width * .8f, Screen.height * .08f), Tooltip, gooeyskin.textArea);
		}
		if(battleBoss.CardsToTarget != 0 && Tooltip == "") {
			GUI.Box(new Rect(Screen.width*.02f, Screen.height*.72f, Screen.width*.8f, Screen.height*.04f), 
					"Please select " + battleBoss.CardsToTarget.ToString() + " cards", gooeyskin.textArea);
		}
	}

	//Update: just for animations
	public virtual void Update() {
		//this section is messy as hell because shuffleanimating should be a separate path but instead takes alternate routes within animating. 
		if(ShuffleAnimating) {
			if(Time.time > DrawStartTime + .48f) {
				//this ends the animation
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
	/// METHODS FOR SELECTING, TARGET, UNTARGET, DRAWING, DISCARDING, 
	/// TUCKING, BURNING, PEEKING CARDS--THESE ARE UNIVERSAL, AND
	/// INCORPORATE ANIMATIONS AS WELL AS GAME LOGIC CHANGES
	//////////////////////////////////////

	//select, deselect
	public void Select() {
		battleBoss.DeselectCards();
		gridBoss.DestroyAllTargetSquares();
		gameControlUI.Dim(false);

		if(battleBoss.PlaysLeft > 0) { 
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
//		if(CardsToTarget > 1) {
			transform.Translate(new Vector3(0f, .25f, 0f));
//		}
		Targeted = true;
	}
	public void Untarget() {
		transform.Translate(new Vector3(0f, -.25f, 0f));
		Targeted = false;
	}
	//draw 
	public void DrawAnimate(int position) {
		gameObject.transform.parent = battleBoss.handObj.transform;
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
		transform.parent = battleBoss.handObj.transform;
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
			battleBoss.Hand.Remove(gameObject);
			battleBoss.Discard.Add(gameObject);
		}
	}
	public void FinishDiscard() {
		DiscardAnimating = false;
		Deselect();
		if(DiscardWhenPlayed | ForcingDiscardOfThis) {
	//		Discarded = true;
	//		battleBoss.Hand.Remove(gameObject);
	//		battleBoss.Discard.Add(gameObject);
			Animating = false;
			BurnAnimating = false;
			DrawAnimating = false;
			ShuffleAnimating = false;

			int index = battleBoss.Discard.IndexOf(gameObject);

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
		
		shopBoss.GoalCheck("Discard pile has X cards in it");
		shopBoss.GoalCheck("Discard pile has X cards in a row with the same God");

		//	i put OrganizeCards into CheckQ
		//	Invoke("OrganizeCards", .3f);
	}

	public virtual void Tuck() {
		Debug.Log("Placing this card back into the deck");

		clickBoss.DisallowEveryInput();
		DrawEndPosition = new Vector3(-2.7f, 3f, 0);
		Animating = true;
		DrawStartTime = Time.time;

		if(battleBoss.Hand.Contains(gameObject)) 
			battleBoss.Hand.Remove(gameObject);
		if (battleBoss.Discard.Contains (gameObject)) 
			battleBoss.Discard.Remove(gameObject);
		if(battleBoss.PeekedCards.Contains(gameObject)) 
			battleBoss.PeekedCards.Remove(gameObject);

		Invoke("FinishTuck", .25f);
	}
	public void FinishTuck(){
		string tempString = CardName;
		tempString.Replace("\n", " ");
		battleBoss.Deck.Add(tempString);
		Destroy(gameObject);

		clickBoss.AllowEveryInput();
	}

	public virtual void Burn() {
		EventControl.EventCheck("Burn");
		clickBoss.DisallowEveryInput();
		if(Selected)
			Deselect();
		battleBoss.Hand.Remove(gameObject);
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

		gameControlUI.Dim(false);

		if(Discarded) {
			return;
		}		

		if(CardAction == CardActionTypes.TargetGridSquare) {
			Activate();
			return;
		}

		if(Selected) {
			//Extreme corner case, this prevents Target Card cards from being played without valid targets
			if(CardAction == CardActionTypes.TargetCard && ((CardsToTargetWillBeDiscarded && battleBoss.Discard.Count < 1) | 
															(!CardsToTargetWillBeDiscarded && battleBoss.Hand.Count < 2)    ) ) {
				Debug.Log("yeah!!!!");
				battleBoss.Tooltip = "You can't play this card right now, because it can't target a card.";
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
			battleBoss.AddPlays(-1);

		clickBoss.DisallowEveryInput();

		switch(CardAction) {
		case CardActionTypes.Armor:
			break;
		case CardActionTypes.NoTarget:
			Play();
			DiscardOrBurnIfNotInQ();
			clickBoss.AllowInputUmbrella = false;
			CheckQ();
			break;
		case CardActionTypes.NoTargetNoInput:
			Play();
			DiscardOrBurnIfNotInQ();
			break;
		case CardActionTypes.NoTargetGridSquare:
			Play();
			DiscardOrBurnIfNotInQ();
			clickBoss.AllowInputUmbrella = false;
			gridBoss.MakeSquares(aoeTargetType, aoeMinRange, aoeMaxRange, false);
			CheckQ();
			break;
		case CardActionTypes.TargetGridSquare:
			EnterTargetingMode();
			clickBoss.AllowEveryInput();
			break;
		case CardActionTypes.TargetCard:
			if((CardsToTargetWillBePeeked && battleBoss.PeekedCards.Count < CardsToTarget) |
			   (CardsToTargetWillBeDiscarded && battleBoss.Discard.Count < CardsToTarget) |
			   (!CardsToTargetWillBeDiscarded && battleBoss.Hand.Count < CardsToTarget)) {
				Tooltip = "Not enough cards to target!";
				DiscardOrBurnIfNotInQ();
				battleBoss.AnimateCardsToCorrectPosition();
				return;
			}

			//VVV by default, the cards to target are not discarded or peeked, but you can change that in the Play()
			clickBoss.AllowInputUmbrella = false;
			ReallowUmbrellaInputAfterDiscardOrBurn();

			clickBoss.DisallowEveryInput();
			clickBoss.AllowCardTargetInput = true;
			clickBoss.AllowInfoInput = true;

			if(CardsToTargetWillBeDiscarded) 
				battleBoss.CardsToTargetAreDiscarded = true;
			else 
				battleBoss.CardsToTargetAreDiscarded = false;

			if(CardsToTargetWillBePeeked) 
				battleBoss.CardsToTargetArePeeked = true;
			else
				battleBoss.CardsToTargetArePeeked = false;

			battleBoss.TargetCardCallback = this;
			
			Play();
			DiscardOrBurnIfNotInQ();
			battleBoss.AnimateCardsToCorrectPosition();
			break;
		case CardActionTypes.Options:
			Play();
			DiscardOrBurnIfNotInQ();
			clickBoss.DisallowEveryInput();
			gameControlUI.Dim();
			break;
		default:
			Debug.Log("FUCK");
			break;
		}
	}
	public void EnterTargetingMode() {
		Select();
		gridBoss.EnterTargetingMode(rangeTargetType, minRange, maxRange);
		battleBoss.TargetSquareCallback = this;
		battleBoss.Tooltip = "Please select a square.";
		
		clickBoss.AllowInputUmbrella = true;
		clickBoss.AllowSquareTargetInput = true;
		clickBoss.AllowInfoInput = true;
	}

	public virtual void Play() {
		shopBoss.GoalCheck("Play X cards in one turn");
		
		if(ThisRarity == Rarity.Paper) {
			shopBoss.GoalCheck("Play X paper cards in one turn");
		}
		
		shopBoss.GoalCheck("Play less than X cards total");
		shopBoss.GoalCheck("Don't play a card X turns in a row");
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
		clickBoss.Invoke("ChangeUmbrellaInputAllowToTrue", .53f);
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
		battleBoss.TargetedCards = new List<GameObject>();
		battleBoss.CardsToTarget = 0;
		gameControlUI.Dim(false);

	//	i put OrganizeCards into CheckQ
	//	Invoke("OrganizeCards", .3f);
		CheckQ();
	}
	//GiveOptions method. First you Play(), which sends options to OptionControl, then you pick an option, then it calls this method
	public virtual void OptionsCalledThis(bool ResponseIsYes) {

		optionBoss.TurnOffOptions();

		//	i put OrganizeCards into CheckQ
		//	Invoke("OrganizeCards", .3f);
		clickBoss.AllowEveryInput();
		CheckQ();
	}
	public virtual void OptionsCalledThis(int Choice) {

		optionBoss.TurnOffOptions();
		
		//	i put OrganizeCards into CheckQ
		//	Invoke("OrganizeCards", .3f);

		clickBoss.AllowEveryInput();
		
		CheckQ();
	}
	//TargetGridSquare methods. First, a bunch of targetsquares are spawned, then when one is clicked on it calls TargetSquareCalledThis which calls FindAndAffectUnits
	//which Affects each unit in the area. Simple battle spells use this.
	public virtual void TargetSquareCalledThis(int x, int y) {
		if(!FreeTargetSquare) {
			DiscardOrBurnIfNotInQ();
			
			battleBoss.AddPlays(-1);

			clickBoss.DisallowEveryInput();
		}

		gridBoss.DestroyAllTargetSquares();
	
		FindAndAffectUnits(x, y);
		gridBoss.MakeSquares(aoeTargetType, aoeMinRange, aoeMaxRange, x, y, false);

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

		gridBoss.FindAllGridUnits();
			
		for(int i = gridBoss.gridUnits.Count-1; i > -1; i--) {
			if(aoeTargetType == GridControl.TargetTypes.diamond) {
				int distance =(Mathf.Abs(clickedX - gridBoss.gridUnits[i].xPosition) + Mathf.Abs(clickedY - gridBoss.gridUnits[i].yPosition));
				if(distance >= aoeMinRange && distance <= aoeMaxRange){
					Affect(gridBoss.gridUnits[i]);
				}
			}
			else if(aoeTargetType == GridControl.TargetTypes.none) {
				if(clickedX == gridBoss.gridUnits[i].xPosition && clickedY == gridBoss.gridUnits[i].yPosition) {
					Affect(gridBoss.gridUnits[i]);
				}
			}
			else if(aoeTargetType == GridControl.TargetTypes.square) {
				int xDifference = Mathf.Abs(clickedX - gridBoss.gridUnits[i].xPosition);
				int yDifference = Mathf.Abs(clickedY - gridBoss.gridUnits[i].yPosition);
				if(xDifference >= aoeMinRange && xDifference <= aoeMaxRange && 
				   yDifference >= aoeMinRange && yDifference <= aoeMaxRange) {
					Affect(gridBoss.gridUnits[i]);
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
	//public void OrganizeCards() {
	//	battleBoss.AnimateCardsToCorrectPosition();
	//	battleBoss.CheckDeckCount ();
	//}
	public int HandIndex() {
		if(battleBoss == null) {
			battleBoss = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
		}
		for(int i = 0; i < battleBoss.Hand.Count; i++) {
			if(battleBoss.Hand[i] == gameObject) {
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