using UnityEngine;
using System.Collections;

public class BookOfHistory : Card {
	
	public override void Initialize ()
	{
		CardName = "Book Of History";
//		ReallowInputAfterCardTargeting = false;

		base.Initialize ();
	}

	public override void Play () {
		S.GameControlInst.CardsToTarget = 1;

		S.GameControlGUIInst.ForceDim ();
		S.GameControlGUIInst.SetTooltip("Pick a card to be played twice.");
		S.ClickControlInst.AllowForfeitButtonInput = true;
		
		base.Play ();
	}

	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();

//			if(tempCard.CardAction == CardActionTypes.TargetCard) {
//				QControl.AddToQ(tempCard, QControl.QMethodType.Discard);
//			}

			QControl.AddToQ(tempCard, QControl.QMethodType.FreeActivate);
			QControl.AddToQ(tempCard, QControl.QMethodType.FreeActivate);

			if(tempCard.CardAction == CardActionTypes.TargetGridSquare) {
				QControl.AddToQ(tempCard, QControl.QMethodType.Discard);
			}
		}

		base.AfterCardTargetingCallback ();
	}
}
