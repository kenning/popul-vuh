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

		S.GameControlInst.Peek (3, this);

		S.GameControlInst.CardsToTarget = 1;
		gameControlGUI.ForceDim ();
		gameControlGUI.SetTooltip("Pick a card to put in your hand.");
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {

		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			S.GameControlInst.DrawIntoHand(tempCard, false);
		}
		
		Invoke ("OrganizeCards", .3f);

		base.AfterCardTargetingCallback ();
	}
}
