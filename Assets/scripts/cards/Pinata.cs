using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pinata : Card {

	//public List<string> cardOptions;
	
	public override void Initialize ()
	{
		CardName = "Pinata";

		base.Initialize ();
	}
	
	public override void Play () {

		gameControl.Tooltip = "";

		gameControl.Peek(2, this);

		base.Play ();
	}

	public override void PeekCallback () {
		string TooltipMessage = "";

		foreach(GameObject go in gameControl.PeekedCards) {
			Card card = go.GetComponent<Card>();
			if(card.ThisRarity == Rarity.Bronze) {
				gameControl.Draw();
				gameControl.Draw();
				TooltipMessage += "+2 cards! ";
			}
			else if(card.ThisRarity == Rarity.Silver) {
				gameControl.AddPlays(2);
				TooltipMessage += "+2 plays! ";
			}
			else if(card.ThisRarity == Rarity.Gold) {
				gameControl.AddDollars(2);
				TooltipMessage += "+2$! ";
			}
			else if(card.ThisRarity == Rarity.Paper) {
				gameControl.AddDollars(1);
				TooltipMessage += "+1$! ";
			}
			TooltipMessage += "\n";
		}
		gameControl.Tooltip = TooltipMessage;

		Invoke ("PinataMethod", 2f);
	}

	void PinataMethod () {
		gameControlGUI.ForceDim();
		for(int i = gameControl.PeekedCards.Count-1; i > -1; i++) {
			gameControl.PeekedCards[i].GetComponent<Card>().Tuck();
		}
		gameControl.Tooltip = "";

		clickControl.AllowEveryInput ();
		CheckQ();
	}
}
