using UnityEngine;
using System.Collections;

public class Nunchucks : Card {

	public override void Initialize ()
	{
		CardName = "Nunchucks";
		TitleFontSize = 30;

		base.Initialize ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}

	public override void TargetSquareCalledThis (int x, int y) {

		for(int i = 0; i < battleBoss.Hand.Count; i++) {
			Card card = battleBoss.Hand[i].GetComponent<Card>();
			if(card.CardName == "Nunchucks") {
				battleBoss.AddPlays(1);
				break;
			}
		}

		base.TargetSquareCalledThis (x, y);
	}
}
