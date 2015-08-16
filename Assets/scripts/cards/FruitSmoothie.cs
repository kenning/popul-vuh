using UnityEngine;
using System.Collections;

public class FruitSmoothie : Card {
	
	public override void Initialize ()
	{
		CardName = "Fruit Smoothie";

		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.Draw ();
		gameControl.Draw ();
		gameControl.Draw ();

		gameControl.CardsToTarget = 2;

		gameControlGUI.ForceDim();
		gameControl.Tooltip = "Pick two cards to tuck back into your deck.";
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in gameControl.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Tuck();
		}

		base.AfterCardTargetingCallback ();
	}
}
