using UnityEngine;
using System.Collections;

public class BookOfMatches : Card {
	
	public override void Initialize ()
	{
		CardName = "Book Of Matches";
		
		base.Initialize ();
	}
	
	public override void Play () {

		S.GameControlInst.CardsToTarget = 1;

		S.GameControlGUIInst.ForceDim ();
		S.GameControlGUIInst.SetTooltip("Pick a card to burn.");
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}
		
//		ReallowEveryInputAfterDiscardOrBurn();
		base.AfterCardTargetingCallback ();
	}
}
