using UnityEngine;
using System.Collections;

public class Soda : Card {
	
	public override void Initialize ()
	{
		CardName = "Soda";
 
		base.Initialize ();
	}
	
	public override void Play () {
		if(gameControl.Hand.Count < 2) {
			Tooltip = "You need a card to discard!";
			return;
		}
		gameControl.CardsToTarget = 1;

		gameControlGUI.Dim(true);
		gameControl.Tooltip = "Pick a card to discard.";
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in gameControl.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.DiscardAnimate();
		}

		gameControl.Draw ();
		gameControl.AddPlays (1);
		gameControl.AddMoves (1);

		base.AfterCardTargetingCallback ();
	}
}
