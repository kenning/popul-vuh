using UnityEngine;
using System.Collections;

public class CharcoalShoes : Card {
	
	public override void Initialize ()
	{
		CardName = "Charcoal Shoes";
		TitleFontSize = 40;
		
		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.CardsToTarget = 1;

		gameControl.Tooltip = "Pick a card to burn.";
		
		gameControlUI.Dim (true);
		
		base.Play ();
	}
	
	public override void AfterCardTargetingCallback() {
		foreach(GameObject tempGO in gameControl.TargetedCards){
			Card tempCard = tempGO.GetComponent<Card>();
			tempCard.Burn();
		}
		
		gameControl.AddMoves (1);

//		ReallowEveryInputAfterDiscardOrBurn ();

		base.AfterCardTargetingCallback ();
	}
}
