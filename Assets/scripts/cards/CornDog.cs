using UnityEngine;
using System.Collections;

public class CornDog : Card {
	
	public override void Initialize ()
	{
		CardName = "Corn Dog";

		base.Initialize ();
	}

	public override void Play () {
		battleBoss.Draw ();
		battleBoss.Draw ();
		battleBoss.AddPlays (1);

		battleBoss.CardsToTarget = 2;

		gameControlUI.Dim(true);
		battleBoss.Tooltip = "Pick two cards to discard.";
		
		base.Play ();
	}

	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in battleBoss.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.DiscardAnimate();
		}

		base.AfterCardTargetingCallback ();
	}
}
