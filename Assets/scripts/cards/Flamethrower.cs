using UnityEngine;
using System.Collections;

public class Flamethrower : Card {
	
	public override void Initialize ()
	{
		CardName = "Flamethrower";
		SmallFontSize = 30;

		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.CardsToTarget = 1;

		gameControl.TargetSquareCallback = this;

		gameControlUI.Dim(true);
		gameControl.Tooltip = "Pick a card to burn.";
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in gameControl.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}

		gridBoss.EnterTargetingMode (rangeTargetType, minRange, maxRange);

		clickBoss.DisallowEveryInput ();
		clickBoss.AllowInfoInput = true;
		clickBoss.AllowSquareTargetInput = true;
		gameControl.TargetSquareCallback = this;
//		ReallowUmbrellaInputAfterDiscardOrBurn ();

		base.AfterCardTargetingCallback ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}

	public override void TargetSquareCalledThis (int x, int y) {
		gridBoss.DestroyAllTargetSquares();
		
		FindAndAffectUnits(x, y);
		gridBoss.MakeSquares (aoeTargetType, aoeMinRange, aoeMaxRange, x, y, false);
		
		clickBoss.DisallowEveryInput ();
//		ReallowEveryInputAfterDiscardOrBurn ();
	}
}
