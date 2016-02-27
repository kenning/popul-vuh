using UnityEngine;
using System.Collections;

public class BurntCoffee : Card {
	
	public override void Initialize ()
	{
		CardName = "Burnt Coffee";
		
		base.Initialize ();
	}
	
	public override void Play () {
		S.GameControlInst.CardsToTarget = 1;

		S.GameControlGUIInst.SetTooltip("Pick a card to burn.");
		
		S.GameControlGUIInst.ForceDim ();

		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}
		
		S.GameControlInst.AddPlays (2);

//		ReallowEveryInputAfterDiscardOrBurn ();

		base.AfterCardTargetingCallback ();
	}
}
