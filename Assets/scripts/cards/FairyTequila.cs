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
		battleBoss.CardsToTarget = 1;

		battleBoss.Dim (true);
		battleBoss.Tooltip =  ("Please select a discarded card to return to your hand.");
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {

		foreach(GameObject tempGO in battleBoss.TargetedCards){
			battleBoss.Return(tempGO);
		}
		
//		ReallowEveryInputAfterDiscardOrBurn();
		base.AfterCardTargetingCallback ();
	}
}
