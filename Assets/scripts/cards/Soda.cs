using UnityEngine;
using System.Collections;

public class Soda : Card {
	
	public override void Initialize ()
	{
		CardName = "Soda";
 
		base.Initialize ();
	}
	
	public override void Play () {
		if(battleBoss.Hand.Count < 2) {
			Tooltip = "You need a card to discard!";
			return;
		}
		battleBoss.CardsToTarget = 1;

		battleBoss.Dim (true);
		battleBoss.Tooltip = "Pick a card to discard.";
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in battleBoss.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.DiscardAnimate();
		}

		battleBoss.Draw ();
		battleBoss.AddPlays (1);
		battleBoss.AddMoves (1);

		base.AfterCardTargetingCallback ();
	}
}
