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
		battleBoss.CardsToTarget = 1;

		battleBoss.TargetSquareCallback = this;

		gameControlUI.Dim(true);
		battleBoss.Tooltip = "Pick a card to burn.";
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in battleBoss.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}

		gridBoss.EnterTargetingMode (rangeTargetType, minRange, maxRange);

		clickBoss.DisallowEveryInput ();
		clickBoss.AllowInfoInput = true;
		clickBoss.AllowSquareTargetInput = true;
		battleBoss.TargetSquareCallback = this;
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
