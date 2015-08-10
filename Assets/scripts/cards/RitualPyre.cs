using UnityEngine;
using System.Collections;

public class RitualPyre : Card {
	
	public override void Initialize ()
	{
		CardName = "Ritual Pyre";

		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.CardsToTarget = 1;

		gameControl.TargetSquareCallback = this;

		gameControl.Tooltip = "Pick a card to burn.";

		gameControlUI.Dim(true);
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in gameControl.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}

		gameControl.AddDollars (3);

		base.AfterCardTargetingCallback ();
	}
}
