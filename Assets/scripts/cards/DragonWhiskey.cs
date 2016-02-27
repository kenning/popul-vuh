using UnityEngine;
using System.Collections;

public class DragonWhiskey : Card {
	
	public override void Initialize ()
	{
		CardName = "Dragon Whiskey";
		base.Initialize ();
	}
	
	public override void Play () {
		//haven't touched anything below this comment yet
		S.GameControlInst.CardsToTarget = 1;

		S.GameControlGUIInst.ForceDim();
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			S.GameControlInst.Return(tempGO);
		}
		
//		ReallowUmbrellaInputAfterDiscardOrBurn();
		Invoke ("OrganizeCards", .6f);
		base.AfterCardTargetingCallback ();
	}
}
