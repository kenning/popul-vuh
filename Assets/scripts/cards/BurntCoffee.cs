using UnityEngine;
using System.Collections;

public class BurntCoffee : Card {
	
	public override void Initialize ()
	{
		CardName = "Burnt Coffee";
		
		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.CardsToTarget = 1;

		gameControl.Tooltip =  "Pick a card to burn.";
		
		gameControlUI.Dim (true);

		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in gameControl.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}
		
		gameControl.AddPlays (2);

//		ReallowEveryInputAfterDiscardOrBurn ();

		base.AfterCardTargetingCallback ();
	}
}
