using UnityEngine;
using System.Collections;

public class BookOfMatches : Card {
	
	public override void Initialize ()
	{
		CardName = "Book Of Matches";
		TitleFontSize = 40;
		
		base.Initialize ();
	}
	
	public override void Play () {

		gameControl.CardsToTarget = 1;

		gameControlUI.Dim (true);
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
