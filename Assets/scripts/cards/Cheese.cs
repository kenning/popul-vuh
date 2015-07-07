using UnityEngine;
using System.Collections;

public class Cheese : Card {
	
	public override void Initialize ()
	{
		CardName = "Cheese";
		SmallFontSize = 30;
		
		base.Initialize ();
	}
	
	public override void Play () {

		battleBoss.CardsToTarget = 1;

		battleBoss.Dim ();
		battleBoss.Tooltip = "Pick a card to burn.";

		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {

		foreach(GameObject tempGO in battleBoss.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}

		battleBoss.Deck.Add ("Cheese");
		
		base.AfterCardTargetingCallback ();
	}

	public override void Burn ()
	{
		battleBoss.Draw ();
        battleBoss.AddPlays(1);

		base.Burn ();
	}
}
