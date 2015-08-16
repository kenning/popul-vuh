using UnityEngine;
using System.Collections;

public class BreadBowl : Card {
	
	public override void Initialize ()
	{
		CardName = "Bread Bowl";
		CardsToTargetWillBePeeked = true;

		base.Initialize ();
	}
	
	public override void Play () {

		gameControl.Peek (3, this);

		gameControl.CardsToTarget = 1;
		gameControlGUI.ForceDim ();
		gameControl.Tooltip = "Pick a card to put in your hand.";
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {

		foreach(GameObject tempGO in gameControl.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			gameControl.DrawIntoHand(tempCard, false);
		}
		
		Invoke ("OrganizeCards", .3f);

		base.AfterCardTargetingCallback ();
	}
}
