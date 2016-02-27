using UnityEngine;
using System.Collections;

public class Cheese : Card {
	
	public override void Initialize ()
	{
		CardName = "Cheese";
		
		base.Initialize ();
	}
	
	public override void Play () {

		S.GameControlInst.CardsToTarget = 1;

		gameControlGUI.Dim();
		gameControlGUI.SetTooltip("Pick a card to burn.");

		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {

		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}

		S.GameControlInst.Deck.Add ("Cheese");
		
		base.AfterCardTargetingCallback ();
	}

	public override void Burn ()
	{
		S.GameControlInst.Draw ();
        S.GameControlInst.AddPlays(1);

		base.Burn ();
	}
}
