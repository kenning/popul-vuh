using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card : MonoBehaviour {

	public enum CardActionTypes { Armor, NoTarget, NoTargetNoInput, NoTargetGridSquare, TargetGridSquare, TargetCard, Options };
	public enum Rarity { Paper, Bronze, Silver, Gold, Platinum };

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
	public GameControlGUI gameControlGUI;
	public ShopControlGUI shopControlGUI;
	public GUIStyleLibrary styleLibrary;

	public CardUI cardUI;

	public CardLibrary library;
	public GameObject hand;
	public GameObject playerObj;

	public int CardsToTarget = 0;
	public bool CardsToTargetWillBeDiscarded = false;
	public bool CardsToTargetWillBePeeked = false;

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
		gameControlGUI = gameController.GetComponent<GameControlGUI> ();
		shopControlGUI = gameController.GetComponent<ShopControlGUI> ();
		eventGUIControl = gameController.GetComponent<EventGUI> ();
		styleLibrary = gameController.GetComponent<GUIStyleLibrary> ();
		cardUI = gameObject.GetComponent<CardUI> ();
		hand = GameObject.Find("Hand");
		playerObj = GameObject.FindGameObjectWithTag("Player");

		// Initialize card variables
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

		gameControl.Hand.Add(gameObject);

		switch (ThisRarity)
		{
		case Card.Rarity.Gold:
			Cost = 10;
			break;
		case Card.Rarity.Silver:
			Cost = 6;
			break;
		case Card.Rarity.Bronze:
			Cost = 3;
			break;
		case Card.Rarity.Paper:
			Cost = 0;
			break;
		}

		cardUI.Initialize (gameController);
    }

	//////////////////////////////////////
	/// Universal card methods
	//////////////////////////////////////

	public void Select() {
		gameControl.DeselectCards();
		gridControl.DestroyAllTargetSquares();
		gameControlGUI.Dim(false);

		if(gameControl.PlaysLeft > 0) { 
			if(gameObject != null) {
				cardUI.TargetAnimate();
				Selected = true;
			}
		}
		else {
			if(CardAction != CardActionTypes.Armor && CardAction != CardActionTypes.TargetGridSquare) {
				cardUI.ErrorAnimate();
			}
		}
	}

	public void Deselect() {
		if (Selected) cardUI.UntargetAnimate ();
		Selected = false;
	}

	public void Target() {
		Targeted = true;
		cardUI.TargetAnimate ();
	}

	public void Untarget() {
		Targeted = false;
		cardUI.UntargetAnimate ();
	}

	public void Discard() {
		if (Discarded) {
			Debug.LogError ("Trying to discard a card which is already marked as discarded. Bug?");
			return;
		}
		bool actuallyDiscarding = DiscardWhenPlayed | ForcingDiscardOfThis;
		if(actuallyDiscarding) {
			Discarded = true;
			gameControl.Hand.Remove(gameObject);
			gameControl.Discard.Add(gameObject);
		}

		cardUI.DiscardAnimate ();
	}

	public void FinishDiscard() {
		Deselect();
		ForcingDiscardOfThis = false;

		bool actuallyDiscarding = DiscardWhenPlayed | ForcingDiscardOfThis;
		cardUI.FinishDiscardAnimate (actuallyDiscarding);

		shopControl.GoalCheck("Discard pile has X cards in it");
		shopControl.GoalCheck("Discard pile has X cards in a row with the same God");
	}

	public virtual void Tuck() {
		if(gameControl.Hand.Contains(gameObject)) 
			gameControl.Hand.Remove(gameObject);
		if (gameControl.Discard.Contains (gameObject)) 
			gameControl.Discard.Remove(gameObject);
		if(gameControl.PeekedCards.Contains(gameObject)) 
			gameControl.PeekedCards.Remove(gameObject);
		
		cardUI.TuckAnimate ();

		clickControl.DisallowEveryInput();
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
		clickControl.DisallowEveryInput();
		if(Selected) Deselect();
		gameControl.Hand.Remove(gameObject);

		cardUI.BurnAnimate ();

		EventControl.EventCheck("Burn");
	}

	public void Peek(int position, int maxPositions) {
		Peeked = true;

		cardUI.PeekAnimate (position, maxPositions);
	}

	public virtual void PeekCallback() {
		// This gets overwritten by inheriting cards
		Debug.Log ("peeked!");
	}

	//////////////////////////////////////
	/// METHODS USED FOR EXECUTING THE CARD--THESE ARE UNIVERSAL
	//////////////////////////////////////
	
	public virtual void Click() {

		gameControlGUI.Dim(false);

		if(Discarded) {
			return;
		}		

		if(CardAction == CardActionTypes.TargetGridSquare) {
			Activate(false);
			return;
		}
		
		if(Selected) {
			if(CardAction == CardActionTypes.Armor) {
				return;
			}
			//Extreme corner case, this prevents Target Card cards from being played without valid targets
			if(CardAction == CardActionTypes.TargetCard && ((CardsToTargetWillBeDiscarded && gameControl.Discard.Count < 1) | 
															(!CardsToTargetWillBeDiscarded && gameControl.Hand.Count < 2)    ) ) {
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
	public virtual void Activate(bool FreePlay) {
		if(!FreePlay && CardAction != CardActionTypes.TargetGridSquare)
			gameControl.AddPlays(-1);

		clickControl.DisallowEveryInput();

		switch(CardAction) {
		case CardActionTypes.Armor:
			Debug.Log ("played armor");
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
				return;
			}

			clickControl.AllowInputUmbrella = false;

			// Reallows input after discard or burn
			clickControl.Invoke("ChangeUmbrellaInputAllowToTrue", .53f);

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
			Debug.Log("got here");
			break;
		case CardActionTypes.Options:
			Play();
			DiscardOrBurnIfNotInQ();
			clickControl.DisallowEveryInput();
			gameControlGUI.Dim();
			break;
		default:
			Debug.Log("Bug!");
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

		gameControlGUI.AnimateCardsToCorrectPositionInSeconds (.3f);
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

		if(!BurnsSelfWhenPlayed && !Discarded) {
			Discard();
		}
	}

	//////////////////////////////////////
	/// QCall methods
	//////////////////////////////////////
	
	public virtual void OptionQCall() {
		Debug.Log("Default OptionQCall method called!");
	}
	public virtual void SpecialQCall() {
		Debug.Log("Default SpecialQCall method called!");
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

	public virtual void AfterCardTargetingCallback() {
		gameControl.TargetedCards = new List<GameObject>();
		gameControl.CardsToTarget = 0;
		gameControlGUI.UnlockDim ();
		gameControlGUI.Dim(false);

		gameControlGUI.AnimateCardsToCorrectPosition ();

		CheckQ();
	}

	//GiveOptions method. First you Play(), which sends options to OptionControl, then you pick an option, then it calls this method
	public virtual void OptionsCalledThis(bool ResponseIsYes) {
		optionControl.TurnOffOptions();
		clickControl.AllowEveryInput();
		CheckQ();
	}
	public virtual void OptionsCalledThis(int Choice) {
		optionControl.TurnOffOptions();
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

		CheckQ();
	}

	// This gets overwritten by some cards
	public virtual void EventCall(string EventName) { 	}

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
				Debug.LogError("the card should have a rangeTargetType!");
		}
	}
	public virtual void Affect(GridUnit x) { Debug.LogError("Card parentclass's Affect() was called. Bad!"); }

	//////////////////////////////////////
	/// Utilities
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

	public void DestroyThisGameObject() {
		EventControl.RemoveFromLists(this);
		Destroy(gameObject);
	}
}