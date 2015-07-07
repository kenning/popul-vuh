using UnityEngine;
using System.Collections;

public class Goulash : Card {
	
	public override void Initialize ()
	{
		CardName = "Goulash";

		SmallFontSize = 32;

		base.Initialize ();
	}
	
	public override void Play () {
		battleBoss.CardsToTarget = 1;

		battleBoss.Dim (true);
		battleBoss.Tooltip = "Pick a card to discard.";

		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in battleBoss.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			if(tempCard.ThisRarity == Rarity.Paper) {
				battleBoss.Draw();
				battleBoss.Draw();
				battleBoss.Draw();
				battleBoss.AddPlays(1);
			}
			else {
				battleBoss.Draw();
				battleBoss.AddPlays(3);
			}
			tempCard.DiscardAnimate();
		}
		
		base.AfterCardTargetingCallback ();
	}
}
