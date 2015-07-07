using UnityEngine;
using System.Collections;

public class BurntCoffee : Card {
	
	public override void Initialize ()
	{
		CardName = "Burnt Coffee";
		
		base.Initialize ();
	}
	
	public override void Play () {
		battleBoss.CardsToTarget = 1;

		battleBoss.Tooltip =  "Pick a card to burn.";
		
		battleBoss.Dim (true);

		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in battleBoss.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}
		
		battleBoss.AddPlays (2);

//		ReallowEveryInputAfterDiscardOrBurn ();

		base.AfterCardTargetingCallback ();
	}
}
