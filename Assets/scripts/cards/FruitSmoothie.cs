using UnityEngine;
using System.Collections;

public class FruitSmoothie : Card {
	
	public override void Initialize ()
	{
		CardName = "Fruit Smoothie";
		TitleFontSize = 30;

		base.Initialize ();
	}
	
	public override void Play () {
		battleBoss.Draw ();
		battleBoss.Draw ();
		battleBoss.Draw ();

		battleBoss.CardsToTarget = 2;

		gameControlUI.Dim(true);
		battleBoss.Tooltip = "Pick two cards to tuck back into your deck.";
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in battleBoss.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Tuck();
		}

		base.AfterCardTargetingCallback ();
	}
}
