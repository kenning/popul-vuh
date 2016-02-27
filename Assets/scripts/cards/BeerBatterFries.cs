using UnityEngine;
using System.Collections;

public class BeerBatterFries : Card {
	
	public override void Initialize ()
	{
		CardName = "Beer Batter Fries";
		CardsToTargetWillBeDiscarded = true;

		base.Initialize ();
	}

	public override void Play () {

		S.GameControlInst.AddPlays (1);
		S.GameControlInst.Draw ();

		S.GameControlInst.CardsToTarget = 1;
		S.GameControlInst.CardsToTargetAreDiscarded = true;

		gameControlGUI.ForceDim ();
		gameControlGUI.SetTooltip("Please select a discarded card to tuck back into your deck.");
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			tempGO.GetComponent<Card>().Tuck();
		}
		
		base.AfterCardTargetingCallback ();
	}
}