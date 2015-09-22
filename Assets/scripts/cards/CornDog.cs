using UnityEngine;
using System.Collections;

public class CornDog : Card {
	
	public override void Initialize ()
	{
		CardName = "Corn Dog";

		base.Initialize ();
	}

	public override void Play () {
		gameControl.Draw ();
		gameControl.Draw ();
		gameControl.AddPlays (1);

		gameControl.CardsToTarget = 2;

		gameControlGUI.ForceDim();
		gameControlGUI.SetTooltip("Pick two cards to discard.");
		
		base.Play ();
	}

	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in gameControl.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Discard();
		}

		base.AfterCardTargetingCallback ();
	}
}
