using UnityEngine;
using System.Collections;

public class BookOfMatches : Card {
	
	public override void Initialize ()
	{
		CardName = "Book Of Matches";
		
		base.Initialize ();
	}
	
	public override void Play () {

		gameControl.CardsToTarget = 1;

		gameControlGUI.ForceDim ();
		gameControl.Tooltip = "Pick a card to burn.";
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in gameControl.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}
		
//		ReallowEveryInputAfterDiscardOrBurn();
		base.AfterCardTargetingCallback ();
	}
}
