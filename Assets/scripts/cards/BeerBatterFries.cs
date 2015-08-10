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

		battleBoss.AddPlays (1);
		battleBoss.Draw ();

		battleBoss.CardsToTarget = 1;
		battleBoss.CardsToTargetAreDiscarded = true;

		gameControlUI.Dim (true);
		battleBoss.Tooltip =  ("Please select a discarded card to tuck back into your deck.");
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in battleBoss.TargetedCards){
			tempGO.GetComponent<Card>().Tuck();
		}
		
		base.AfterCardTargetingCallback ();
	}
}