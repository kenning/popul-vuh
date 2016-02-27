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

		gameControlGUI.SetTooltip ("");

		S.GameControlInst.Peek(2, this);

		base.Play ();
	}

	public override void PeekCallback () {
		string TooltipMessage = "";

		foreach(GameObject go in S.GameControlInst.PeekedCards) {
			Card card = go.GetComponent<Card>();
			if(card.ThisRarity == Rarity.Bronze) {
				S.GameControlInst.Draw();
				S.GameControlInst.Draw();
				TooltipMessage += "+2 cards! ";
			}
			else if(card.ThisRarity == Rarity.Silver) {
				S.GameControlInst.AddPlays(2);
				TooltipMessage += "+2 plays! ";
			}
			else if(card.ThisRarity == Rarity.Gold) {
				S.GameControlInst.AddDollars(2);
				TooltipMessage += "+2$! ";
			}
			else if(card.ThisRarity == Rarity.Paper) {
				S.GameControlInst.AddDollars(1);
				TooltipMessage += "+1$! ";
			}
			TooltipMessage += "\n";
		}
		gameControlGUI.SetTooltip(TooltipMessage);

		Invoke ("PinataMethod", 2f);
	}

	void PinataMethod () {
		gameControlGUI.ForceDim();
		for(int i = S.GameControlInst.PeekedCards.Count-1; i > -1; i++) {
			S.GameControlInst.PeekedCards[i].GetComponent<Card>().Tuck();
		}
		gameControlGUI.SetTooltip("");

		clickControl.AllowEveryInput ();
		CheckQ();
	}
}
