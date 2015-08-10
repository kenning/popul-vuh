using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pinata : Card {

	//public List<string> cardOptions;
	
	public override void Initialize ()
	{
		CardName = "Pinata";
		SmallFontSize = 32;

		base.Initialize ();
	}
	
	public override void Play () {

		battleBoss.Tooltip = "";

		battleBoss.Peek(2, this);

		base.Play ();
	}

	public override void PeekCallback () {
		string TooltipMessage = "";

		foreach(GameObject go in battleBoss.PeekedCards) {
			Card card = go.GetComponent<Card>();
			if(card.ThisRarity == Rarity.Copper) {
				battleBoss.Draw();
				battleBoss.Draw();
				TooltipMessage += "+2 cards! ";
			}
			else if(card.ThisRarity == Rarity.Silver) {
				battleBoss.AddPlays(2);
				TooltipMessage += "+2 plays! ";
			}
			else if(card.ThisRarity == Rarity.Gold) {
				battleBoss.AddDollars(2);
				TooltipMessage += "+2$! ";
			}
			else if(card.ThisRarity == Rarity.Paper) {
				battleBoss.AddDollars(1);
				TooltipMessage += "+1$! ";
			}
			TooltipMessage += "\n";
		}
		battleBoss.Tooltip = TooltipMessage;

		Invoke ("PinataMethod", 2f);
	}

	void PinataMethod () {
		gameControlUI.Dim(false);
		for(int i = battleBoss.PeekedCards.Count-1; i > -1; i++) {
			battleBoss.PeekedCards[i].GetComponent<Card>().Tuck();
		}
		battleBoss.Tooltip = "";

		clickBoss.AllowEveryInput ();
		CheckQ();
	}
}
