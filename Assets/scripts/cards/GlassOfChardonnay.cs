using UnityEngine;
using System.Collections;

public class GlassOfChardonnay : Card {
	
	public override void Initialize ()
	{
		CardName = "Glass Of Chardonnay";
		CardsToTargetWillBeDiscarded = true;

		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.CardsToTarget = 1;

		gameControlGUI.Dim(true);
		gameControl.Tooltip =  ("Please select a discarded card to tuck back into your deck.");
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in gameControl.TargetedCards){
			tempGO.GetComponent<Card>().Tuck();
		}
		
		gridControl.EnterTargetingMode(rangeTargetType, minRange, maxRange);

		//these must be done manually because after targeting a card, you must finish the card action by attacking.
		clickControl.DisallowEveryInput ();
		clickControl.AllowInfoInput = true;
		clickControl.AllowSquareTargetInput = true;
		gameControl.TargetSquareCallback = this;

		base.AfterCardTargetingCallback ();
	}

	public override void TargetSquareCalledThis (int x, int y) {
		gridControl.DestroyAllTargetSquares();
		
		FindAndAffectUnits(x, y);
		gridControl.MakeSquares (aoeTargetType, aoeMinRange, aoeMaxRange, x, y, false);
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}
}
