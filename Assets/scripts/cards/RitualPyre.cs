using UnityEngine;
using System.Collections;

public class RitualPyre : Card {
	
	public override void Initialize ()
	{
		CardName = "Ritual Pyre";

		base.Initialize ();
	}
	
	public override void Play () {
		battleBoss.CardsToTarget = 1;

		battleBoss.TargetSquareCallback = this;

		battleBoss.Tooltip = "Pick a card to burn.";

		battleBoss.Dim (true);
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in battleBoss.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}

		battleBoss.AddDollars (3);

		base.AfterCardTargetingCallback ();
	}
}
