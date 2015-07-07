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

		battleBoss.Peek (3, this);

		battleBoss.CardsToTarget = 1;
		battleBoss.Dim (true);
		battleBoss.Tooltip = "Pick a card to put in your hand.";
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {

		foreach(GameObject tempGO in battleBoss.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			battleBoss.DrawIntoHand(tempCard, false);
		}
		
		Invoke ("OrganizeCards", .3f);

		base.AfterCardTargetingCallback ();
	}
}
