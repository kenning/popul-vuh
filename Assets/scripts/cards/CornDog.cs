using UnityEngine;
using System.Collections;

public class CornDog : Card {
	
	public override void Initialize ()
	{
		CardName = "Corn Dog";

		base.Initialize ();
	}

	public override void Play () {
		S.GameControlInst.Draw ();
		S.GameControlInst.Draw ();
		S.GameControlInst.AddPlays (1);

		S.GameControlInst.CardsToTarget = 2;

		gameControlGUI.ForceDim();
		gameControlGUI.SetTooltip("Pick two cards to discard.");
		
		base.Play ();
	}

	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Discard();
		}

		base.AfterCardTargetingCallback ();
	}
}
