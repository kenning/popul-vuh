using UnityEngine;
using System.Collections;

public class Flamethrower : Card {
	
	public override void Initialize ()
	{
		CardName = "Flamethrower";

		base.Initialize ();
	}
	
	public override void Play () {
		S.GameControlInst.CardsToTarget = 1;

		S.GameControlInst.TargetSquareCallback = this;

		gameControlGUI.ForceDim();
		gameControlGUI.SetTooltip("Pick a card to burn.");
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}

		gridControl.EnterTargetingMode (rangeTargetType, minRange, maxRange);

		clickControl.DisallowEveryInput ();
		clickControl.AllowInfoInput = true;
		clickControl.AllowSquareTargetInput = true;
		S.GameControlInst.TargetSquareCallback = this;
//		ReallowUmbrellaInputAfterDiscardOrBurn ();

		base.AfterCardTargetingCallback ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}

	public override void TargetSquareCalledThis (int x, int y) {
		gridControl.DestroyAllTargetSquares();
		
		FindAndAffectUnits(x, y);
		gridControl.MakeSquares (aoeTargetType, aoeMinRange, aoeMaxRange, x, y, false);
		
		clickControl.DisallowEveryInput ();
//		ReallowEveryInputAfterDiscardOrBurn ();
	}
}
