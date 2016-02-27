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

		S.GameControlGUIInst.ForceDim();
		S.GameControlGUIInst.SetTooltip("Pick a card to burn.");
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}

		S.GridControlInst.EnterTargetingMode (rangeTargetType, minRange, maxRange);

		S.ClickControlInst.DisallowEveryInput ();
		S.ClickControlInst.AllowInfoInput = true;
		S.ClickControlInst.AllowSquareTargetInput = true;
		S.GameControlInst.TargetSquareCallback = this;
//		ReallowUmbrellaInputAfterDiscardOrBurn ();

		base.AfterCardTargetingCallback ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}

	public override void TargetSquareCalledThis (int x, int y) {
		S.GridControlInst.DestroyAllTargetSquares();
		
		FindAndAffectUnits(x, y);
		S.GridControlInst.MakeSquares (aoeTargetType, aoeMinRange, aoeMaxRange, x, y, false);
		
		S.ClickControlInst.DisallowEveryInput ();
//		ReallowEveryInputAfterDiscardOrBurn ();
	}
}
