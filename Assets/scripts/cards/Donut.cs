using UnityEngine;
using System.Collections;

public class Donut : Card {
	
	public override void Initialize ()
	{
		CardName = "Donut";
		
		base.Initialize ();
	}
	
	public override void Play () {
		S.GameControlInst.Draw ();

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
		
		S.GameControlInst.AddPlays (1);
		S.GameControlInst.AddMoves (2);
		
		base.AfterCardTargetingCallback ();
	}
}
