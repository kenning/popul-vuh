using UnityEngine;
using System.Collections;

public class RitualPyre : Card {
	
	public override void Initialize ()
	{
		CardName = "Ritual Pyre";

		base.Initialize ();
	}
	
	public override void Play () {
		S.GameControlInst.CardsToTarget = 1;

		S.GameControlInst.TargetSquareCallback = this;

		gameControlGUI.SetTooltip("Pick a card to burn.");

		gameControlGUI.ForceDim();
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}

		S.GameControlInst.AddDollars (3);

		base.AfterCardTargetingCallback ();
	}
}
