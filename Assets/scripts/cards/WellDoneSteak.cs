using UnityEngine;
using System.Collections;

public class WellDoneSteak : Card {
	
	public override void Initialize ()
	{
		CardName = "Well Done Steak";
		
		base.Initialize ();
	}
	
	public override void Play () {
		S.GameControlInst.CardsToTarget = 1;

		S.GameControlInst.TargetSquareCallback = this;
		
		S.GameControlGUIInst.SetTooltip("Please select a card to burn.");
		
		S.GameControlGUIInst.ForceDim();
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}
		
		S.GameControlInst.Draw ();
		S.GameControlInst.Draw ();
		S.GameControlInst.AddPlays (1);

		base.AfterCardTargetingCallback ();
	}
}
