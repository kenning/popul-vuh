using UnityEngine;
using System.Collections;

public class Soda : Card {
	
	public override void Initialize ()
	{
		CardName = "Soda";
 
		base.Initialize ();
	}
	
	public override void Play () {
		if(S.GameControlInst.Hand.Count < 2) {
			Tooltip = "You need a card to discard!";
			return;
		}
		S.GameControlInst.CardsToTarget = 1;

		S.GameControlGUIInst.ForceDim();
		S.GameControlGUIInst.SetTooltip("Pick a card to discard.");
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Discard();
		}

		S.GameControlInst.Draw ();
		S.GameControlInst.AddPlays (1);
		S.GameControlInst.AddMoves (1);

		base.AfterCardTargetingCallback ();
	}
}
