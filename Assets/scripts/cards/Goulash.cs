﻿using UnityEngine;
using System.Collections;

public class Goulash : Card {
	
	public override void Initialize ()
	{
		CardName = "Goulash";

		SmallFontSize = 32;

		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.CardsToTarget = 1;

		gameControlGUI.Dim(true);
		gameControl.Tooltip = "Pick a card to discard.";

		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in gameControl.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			if(tempCard.ThisRarity == Rarity.Paper) {
				gameControl.Draw();
				gameControl.Draw();
				gameControl.Draw();
				gameControl.AddPlays(1);
			}
			else {
				gameControl.Draw();
				gameControl.AddPlays(3);
			}
			tempCard.Discard();
		}
		
		base.AfterCardTargetingCallback ();
	}
}
