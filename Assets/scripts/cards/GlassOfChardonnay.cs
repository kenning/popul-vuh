using UnityEngine;
using System.Collections;

public class GlassOfChardonnay : Card {
	
	public override void Initialize ()
	{
		CardName = "Glass Of Chardonnay";
		CardsToTargetWillBeDiscarded = true;
		TitleFontSize = 35;

		base.Initialize ();
	}
	
	public override void Play () {
		battleBoss.CardsToTarget = 1;

		battleBoss.Dim (true);
		battleBoss.Tooltip =  ("Please select a discarded card to tuck back into your deck.");
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in battleBoss.TargetedCards){
			tempGO.GetComponent<Card>().Tuck();
		}
		
		gridBoss.EnterTargetingMode(rangeTargetType, minRange, maxRange);

		//these must be done manually because after targeting a card, you must finish the card action by attacking.
		clickBoss.DisallowEveryInput ();
		clickBoss.AllowInfoInput = true;
		clickBoss.AllowSquareTargetInput = true;
		battleBoss.TargetSquareCallback = this;

		base.AfterCardTargetingCallback ();
	}

	public override void TargetSquareCalledThis (int x, int y) {
		gridBoss.DestroyAllTargetSquares();
		
		FindAndAffectUnits(x, y);
		gridBoss.MakeSquares (aoeTargetType, aoeMinRange, aoeMaxRange, x, y, false);
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}
}
