using UnityEngine;
using System.Collections;

public class FruitSmoothie : Card {
	
	public override void Initialize ()
	{
		CardName = "Fruit Smoothie";

		base.Initialize ();
	}
	
	public override void Play () {
		S.GameControlInst.Draw ();
		S.GameControlInst.Draw ();
		S.GameControlInst.Draw ();

		S.GameControlInst.CardsToTarget = 2;

		gameControlGUI.ForceDim();
		gameControlGUI.SetTooltip("Pick two cards to tuck back into your deck.");
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Tuck();
		}

		base.AfterCardTargetingCallback ();
	}
}
