using UnityEngine;
using System.Collections;

public class Goulash : Card {
	
	public override void Initialize ()
	{
		CardName = "Goulash";


		base.Initialize ();
	}
	
	public override void Play () {
		S.GameControlInst.CardsToTarget = 1;

		S.GameControlGUIInst.ForceDim();
		S.GameControlGUIInst.SetTooltip("Pick a card to discard.");

		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in S.GameControlInst.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			if(tempCard.ThisRarity == Rarity.Paper) {
				S.GameControlInst.Draw();
				S.GameControlInst.Draw();
				S.GameControlInst.Draw();
				S.GameControlInst.AddPlays(1);
			}
			else {
				S.GameControlInst.Draw();
				S.GameControlInst.AddPlays(3);
			}
			tempCard.Discard();
		}
		
		base.AfterCardTargetingCallback ();
	}
}
