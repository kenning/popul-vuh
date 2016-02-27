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
		S.GameControlInst.CardsToTarget = 1;

		S.GameControlGUIInst.ForceDim();
		S.GameControlGUIInst.SetTooltip("Please select a discarded card to return to your hand.");
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {

		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			S.GameControlInst.Return(tempGO);
		}
		
//		ReallowEveryInputAfterDiscardOrBurn();
		base.AfterCardTargetingCallback ();
	}
}
