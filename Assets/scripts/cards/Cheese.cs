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

		gameControl.CardsToTarget = 1;

		gameControlGUI.Dim();
		gameControl.Tooltip = "Pick a card to burn.";

		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {

		foreach(GameObject tempGO in gameControl.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}

		gameControl.Deck.Add ("Cheese");
		
		base.AfterCardTargetingCallback ();
	}

	public override void Burn ()
	{
		gameControl.Draw ();
        gameControl.AddPlays(1);

		base.Burn ();
	}
}
