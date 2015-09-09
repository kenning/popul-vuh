using UnityEngine;
using System.Collections;

public class Donut : Card {
	
	public override void Initialize ()
	{
		CardName = "Donut";
		
		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.Draw ();

		gameControl.CardsToTarget = 1;

		gameControlGUI.ForceDim();
		gameControl.Tooltip = "Pick a card to discard.";


		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in gameControl.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Discard();
		}
		
		gameControl.AddPlays (1);
		gameControl.AddMoves (2);
		
		base.AfterCardTargetingCallback ();
	}
}
