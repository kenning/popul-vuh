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
		battleBoss.CardsToTarget = 1;

		gameControlUI.Dim (true);
		battleBoss.Tooltip = "Pick a card to be played twice.";
		clickBoss.AllowForfeitButtonInput = true;
		
		base.Play ();
	}

	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in battleBoss.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();

//			if(tempCard.CardAction == CardActionTypes.TargetCard) {
//				QControl.AddToQ(tempCard, QControl.QMethodType.Discard);
//			}
//
			QControl.AddToQ(tempCard, QControl.QMethodType.FreeActivate);
			QControl.AddToQ(tempCard, QControl.QMethodType.FreeActivate);

			//TODO TEST THIS
			if(tempCard.CardAction == CardActionTypes.TargetGridSquare) {
				QControl.AddToQ(tempCard, QControl.QMethodType.Discard);
			}
		}

		base.AfterCardTargetingCallback ();
	}
}
