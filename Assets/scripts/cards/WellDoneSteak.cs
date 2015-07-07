using UnityEngine;
using System.Collections;

public class WellDoneSteak : Card {
	
	public override void Initialize ()
	{
		CardName = "Well Done Steak";
		
		base.Initialize ();
	}
	
	public override void Play () {
		battleBoss.CardsToTarget = 1;

		battleBoss.TargetSquareCallback = this;
		
		battleBoss.Tooltip =  ("Please select a card to burn.");
		
		battleBoss.Dim (true);
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in battleBoss.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}
		
		battleBoss.Draw ();
		battleBoss.Draw ();
		battleBoss.AddPlays (1);

		base.AfterCardTargetingCallback ();
	}
}
