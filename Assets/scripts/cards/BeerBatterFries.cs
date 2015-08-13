using UnityEngine;
using System.Collections;

public class BeerBatterFries : Card {
	
	public override void Initialize ()
	{
		CardName = "Beer Batter Fries";
		TitleFontSize = 30;
		CardsToTargetWillBeDiscarded = true;

		base.Initialize ();
	}

	public override void Play () {

		gameControl.AddPlays (1);
		gameControl.Draw ();

		gameControl.CardsToTarget = 1;
		gameControl.CardsToTargetAreDiscarded = true;

		gameControlGUI.Dim (true);
		gameControl.Tooltip =  ("Please select a discarded card to tuck back into your deck.");
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in gameControl.TargetedCards){
			tempGO.GetComponent<Card>().Tuck();
		}
		
		base.AfterCardTargetingCallback ();
	}
}