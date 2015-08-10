using UnityEngine;
using System.Collections;

public class FairyTequila : Card {
	
	public override void Initialize ()
	{
		CardName = "Fairy Tequila";
		CardsToTargetWillBeDiscarded = true;

		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.CardsToTarget = 1;

		gameControlUI.Dim(true);
		gameControl.Tooltip =  ("Please select a discarded card to return to your hand.");
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {

		foreach(GameObject tempGO in gameControl.TargetedCards){
			gameControl.Return(tempGO);
		}
		
//		ReallowEveryInputAfterDiscardOrBurn();
		base.AfterCardTargetingCallback ();
	}
}
