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
		S.GameControlInst.CardsToTarget = 1;

		S.GameControlGUIInst.ForceDim();
		S.GameControlGUIInst.SetTooltip("Please select a discarded card to tuck back into your deck.");
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			tempGO.GetComponent<Card>().Tuck();
		}
		
		S.GridControlInst.EnterTargetingMode(rangeTargetType, minRange, maxRange);

		//these must be done manually because after targeting a card, you must finish the card action by attacking.
		S.ClickControlInst.DisallowEveryInput ();
		S.ClickControlInst.AllowInfoInput = true;
		S.ClickControlInst.AllowSquareTargetInput = true;
		S.GameControlInst.TargetSquareCallback = this;

		base.AfterCardTargetingCallback ();
	}

	public override void TargetSquareCalledThis (int x, int y) {
		S.GridControlInst.DestroyAllTargetSquares();
		
		FindAndAffectUnits(x, y);
		S.GridControlInst.MakeSquares (aoeTargetType, aoeMinRange, aoeMaxRange, x, y, false);
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}
}
