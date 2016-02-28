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

	public CardUI cardUI;
	public CardSFX cardSFX;

	public GameObject playerObj;

	public int CardsToTarget = 0;
	public bool CardsToTargetWillBeDiscarded = false;
	public bool CardsToTargetWillBePeeked = false;

	//Card-specific variables, to be set in the Start() function before calling base.Start()
	public LibraryCard ThisLibraryCard;
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

    public virtual void Initialize() {Initialize(false);}
	public virtual void Initialize(bool alreadyDiscarded) {
        useGUILayout = false;

		cardUI = gameObject.GetComponent<CardUI> ();
		cardSFX = gameObject.GetComponent<CardSFX> ();
		playerObj = GameObject.FindGameObjectWithTag("Player");

		// Initialize card variables
		ThisLibraryCard = CardLibrary.Lib[CardName];

		name = CardName;
		CardName = ThisLibraryCard.CardName;
		DisplayName = ThisLibraryCard.DisplayName;
		IconPath = "sprites/card icons/" + ThisLibraryCard.IconPath;
		Tooltip = ThisLibraryCard.Tooltip;
		DisplayText = ThisLibraryCard.DisplayText;
		MiniDisplayText = ThisLibraryCard.MiniDisplayText;
		rangeTargetType = ThisLibraryCard.RangeTargetType;
		minRange = ThisLibraryCard.rangeMin;
		maxRange = ThisLibraryCard.rangeMax;
		aoeTargetType = ThisLibraryCard.AoeTargetType;
		aoeMinRange = ThisLibraryCard.aoeMinRange;
		aoeMaxRange = ThisLibraryCard.aoeMaxRange;
		ThisRarity = ThisLibraryCard.ThisRarity;
		CardAction = ThisLibraryCard.CardAction;
		God = ThisLibraryCard.God;

        if (!alreadyDiscarded) {
    		S.GameControlInst.Hand.Add(gameObject);        
        }

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

		cardUI.Initialize (alreadyDiscarded);

		cardSFX.PlayDrawCardSFX();
    }

	//////////////////////////////////////
	/// Universal card methods
	//////////////////////////////////////

	public void Select() {
		S.GameControlInst.DeselectCards();
		S.GridControlInst.DestroyAllTargetSquares();
		S.GameControlGUIInst.Dim(false);

		if(S.GameControlInst.PlaysLeft > 0) { 
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
			S.GameControlInst.Hand.Remove(gameObject);
			S.GameControlInst.Discard.Add(gameObject);
            Discarded = true;
			StateSavingControl.Save();
		}

		cardUI.DiscardAnimate ();
	}

	public void InvisibleDiscard() {
		Discarded = true;
		S.GameControlInst.Discard.Add(gameObject);
		cardUI.FinishDiscardAnimate (true);
	}

	public void FinishDiscard() {
		Deselect();
		ForcingDiscardOfThis = false;

		S.ShopControlInst.GoalCheck("Discard pile has X cards in it");
		S.ShopControlInst.GoalCheck("Discard pile has X cards in a row with the same God");
	}

	public virtual void Tuck() {
		if(S.GameControlInst.Hand.Contains(gameObject)) 
			S.GameControlInst.Hand.Remove(gameObject);
		if (S.GameControlInst.Discard.Contains (gameObject)) 
			S.GameControlInst.Discard.Remove(gameObject);
		if(S.GameControlInst.PeekedCards.Contains(gameObject)) 
			S.GameControlInst.PeekedCards.Remove(gameObject);
		
		cardUI.TuckAnimate ();

		S.ClickControlInst.DisallowEveryInput();
		Invoke("FinishTuck", .25f);
	}

	public void FinishTuck(){
		string tempString = CardName;
		tempString.Replace("\n", " ");
		S.GameControlInst.Deck.Add(tempString);
		Destroy(gameObject);

		S.ClickControlInst.AllowEveryInput();
	}

	public virtual void Burn() {
		S.ClickControlInst.DisallowEveryInput();
		if(Selected) Deselect();
		S.GameControlInst.Hand.Remove(gameObject);

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

		S.GameControlGUIInst.Dim(false);

		S.GameControlGUIInst.SetTooltip("");

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
			if(CardAction == CardActionTypes.TargetCard && ((CardsToTargetWillBeDiscarded && S.GameControlInst.Discard.Count < 1) | 
															(!CardsToTargetWillBeDiscarded && S.GameControlInst.Hand.Count < 2)    ) ) {
				S.GameControlGUIInst.SetTooltip("You can't play this card right now, because it can't target a card.");
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
		if(!FreePlay && CardAction != CardActionTypes.TargetGridSquare) {
			S.GameControlInst.AddPlays(-1);
		}

		S.ClickControlInst.DisallowEveryInput();

		switch(CardAction) {
		case CardActionTypes.Armor:
			Debug.Log ("played armor");
			break;
		case CardActionTypes.NoTarget:
			Play();
			DiscardOrBurnIfNotInQ();
			S.ClickControlInst.AllowInputUmbrella = false;
			CheckQ();
			break;
		case CardActionTypes.NoTargetNoInput:
			Play();
			DiscardOrBurnIfNotInQ();
			break;
		case CardActionTypes.NoTargetGridSquare:
			Play();
			DiscardOrBurnIfNotInQ();
			S.ClickControlInst.AllowInputUmbrella = false;
			S.GridControlInst.MakeSquares(aoeTargetType, aoeMinRange, aoeMaxRange, false);
			CheckQ();
			break;
		case CardActionTypes.TargetGridSquare:
			EnterTargetingMode();
			S.ClickControlInst.AllowEveryInput();
			break;
		case CardActionTypes.TargetCard:
			if((CardsToTargetWillBePeeked && S.GameControlInst.PeekedCards.Count < CardsToTarget) |
			   (CardsToTargetWillBeDiscarded && S.GameControlInst.Discard.Count < CardsToTarget) |
			   (!CardsToTargetWillBeDiscarded && S.GameControlInst.Hand.Count < CardsToTarget)) {
				S.GameControlGUIInst.SetTooltip("Not enough cards to target!");
				DiscardOrBurnIfNotInQ();
				return;
			}

			S.ClickControlInst.AllowInputUmbrella = false;

			// Reallows input after discard or burn
			S.ClickControlInst.Invoke("ChangeUmbrellaInputAllowToTrue", .53f);

			S.ClickControlInst.DisallowEveryInput();
			S.ClickControlInst.AllowCardTargetInput = true;
			S.ClickControlInst.AllowInfoInput = true;

			if(CardsToTargetWillBeDiscarded) 
				S.GameControlInst.CardsToTargetAreDiscarded = true;
			else 
				S.GameControlInst.CardsToTargetAreDiscarded = false;

			if(CardsToTargetWillBePeeked) 
				S.GameControlInst.CardsToTargetArePeeked = true;
			else
				S.GameControlInst.CardsToTargetArePeeked = false;

			S.GameControlInst.TargetCardCallback = this;
			
			Play();
			DiscardOrBurnIfNotInQ();
			Debug.Log("got here");
			break;
		case CardActionTypes.Options:
			Play();
			DiscardOrBurnIfNotInQ();
			S.ClickControlInst.DisallowEveryInput();
			S.GameControlGUIInst.Dim();
			break;
		default:
			Debug.Log("Bug!");
			break;
		}
	}

	public void EnterTargetingMode() {
		Select();
		S.GridControlInst.EnterTargetingMode(rangeTargetType, minRange, maxRange);
		S.GameControlInst.TargetSquareCallback = this;
		S.GameControlGUIInst.SetTooltip("Please select a square.");
		
		S.ClickControlInst.AllowInputUmbrella = true;
		S.ClickControlInst.AllowSquareTargetInput = true;
		S.ClickControlInst.AllowInfoInput = true;
	}

	public virtual void Play() {
		cardSFX.PlayPlayCardSFX();

		S.ShopControlInst.GoalCheck("Play X cards in one turn");
		
		if(ThisRarity == Rarity.Paper) {
			S.ShopControlInst.GoalCheck("Play X paper cards in one turn");
		}
		
		S.ShopControlInst.GoalCheck("Play less than X cards total");
		S.ShopControlInst.GoalCheck("Don't play a card X turns in a row");

		S.GameControlGUIInst.AnimateCardsToCorrectPositionInSeconds (.3f);
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
		S.GameControlInst.TargetedCards = new List<GameObject>();
		S.GameControlInst.CardsToTarget = 0;
		S.GameControlGUIInst.UnlockDim ();
		S.GameControlGUIInst.Dim(false);

		S.GameControlGUIInst.AnimateCardsToCorrectPosition ();

		CheckQ();
	}

	//GiveOptions method. First you Play(), which sends options to OptionControl, then you pick an option, then it calls this method
	public virtual void OptionsCalledThis(bool ResponseIsYes) {
		S.OptionControlInst.TurnOffOptions();
		S.ClickControlInst.AllowEveryInput();
		CheckQ();
	}
	public virtual void OptionsCalledThis(int Choice) {
		S.OptionControlInst.TurnOffOptions();
		S.ClickControlInst.AllowEveryInput();
		CheckQ();
	}

	//TargetGridSquare methods. First, a bunch of targetsquares are spawned, then when one is clicked on it calls TargetSquareCalledThis which calls FindAndAffectUnits
	//which Affects each unit in the area. Simple battle spells use this.
	public virtual void TargetSquareCalledThis(int x, int y) {
		if(!FreeTargetSquare) {
			DiscardOrBurnIfNotInQ();
			
			S.GameControlInst.AddPlays(-1);

			S.ClickControlInst.DisallowEveryInput();
		}

		S.GridControlInst.DestroyAllTargetSquares();
	
		FindAndAffectUnits(x, y);
		S.GridControlInst.MakeSquares(aoeTargetType, aoeMinRange, aoeMaxRange, x, y, false);

		CheckQ();
	}

	// This gets overwritten by some cards
	public virtual void EventCall(string EventName) { 	}

	public void FindAndAffectUnits(int clickedX, int clickedY){

		S.GridControlInst.FindAllGridUnits();
			
		for(int i = S.GridControlInst.gridUnits.Count-1; i > -1; i--) {
			if(aoeTargetType == GridControl.TargetTypes.diamond) {
				int distance =(Mathf.Abs(clickedX - S.GridControlInst.gridUnits[i].xPosition) + Mathf.Abs(clickedY - S.GridControlInst.gridUnits[i].yPosition));
				if(distance >= aoeMinRange && distance <= aoeMaxRange){
					Affect(S.GridControlInst.gridUnits[i]);
				}
			}
			else if(aoeTargetType == GridControl.TargetTypes.none) {
				if(clickedX == S.GridControlInst.gridUnits[i].xPosition && clickedY == S.GridControlInst.gridUnits[i].yPosition) {
					Affect(S.GridControlInst.gridUnits[i]);
				}
			}
			else if(aoeTargetType == GridControl.TargetTypes.square) {
				int xDifference = Mathf.Abs(clickedX - S.GridControlInst.gridUnits[i].xPosition);
				int yDifference = Mathf.Abs(clickedY - S.GridControlInst.gridUnits[i].yPosition);
				if(xDifference >= aoeMinRange && xDifference <= aoeMaxRange && 
				   yDifference >= aoeMinRange && yDifference <= aoeMaxRange) {
					Affect(S.GridControlInst.gridUnits[i]);
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
		for(int i = 0; i < S.GameControlInst.Hand.Count; i++) {
			if(S.GameControlInst.Hand[i] == gameObject) {
				return i;
			}
		}
		return 555;
	}

	public void AddToTriggerList() {
		EventControl.AddToTriggerList (this);
	}

	public void RemoveFromTriggerList() {
		EventControl.RemoveFromLists(this);
	}

	public void DestroyThisGameObject() {
		RemoveFromTriggerList();
		StateSavingControl.RemoveFromTriggerList(CardName);
		Destroy(gameObject);
	}
}