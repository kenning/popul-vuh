using UnityEngine;
using System.Collections;

public class Donut : Card {
	
	public override void Initialize ()
	{
		CardName = "Donut";
		
		base.Initialize ();
	}
	
	public override void Play () {
		battleBoss.Draw ();

		battleBoss.CardsToTarget = 1;

		gameControlUI.Dim(true);
		battleBoss.Tooltip = "Pick a card to discard.";
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in battleBoss.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.DiscardAnimate();
		}
		
		battleBoss.AddPlays (1);
		battleBoss.AddMoves (2);
		
		base.AfterCardTargetingCallback ();
	}
}
