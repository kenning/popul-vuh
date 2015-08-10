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
		battleBoss.CardsToTarget = 1;

		gameControlUI.Dim(true);
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in battleBoss.TargetedCards){
			battleBoss.Return(tempGO);
		}
		
//		ReallowUmbrellaInputAfterDiscardOrBurn();
		Invoke ("OrganizeCards", .6f);
		base.AfterCardTargetingCallback ();
	}
}
