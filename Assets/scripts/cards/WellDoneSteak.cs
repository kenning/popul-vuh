using UnityEngine;
using System.Collections;

public class WellDoneSteak : Card {
	
	public override void Initialize ()
	{
		CardName = "Well Done Steak";
		
		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.CardsToTarget = 1;

		gameControl.TargetSquareCallback = this;
		
		gameControl.Tooltip =  ("Please select a card to burn.");
		
		gameControlUI.Dim(true);
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in gameControl.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}
		
		gameControl.Draw ();
		gameControl.Draw ();
		gameControl.AddPlays (1);

		base.AfterCardTargetingCallback ();
	}
}
