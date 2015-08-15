using UnityEngine;
using System.Collections;

public class Nunchucks : Card {

	public override void Initialize ()
	{
		CardName = "Nunchucks";

		base.Initialize ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}

	public override void TargetSquareCalledThis (int x, int y) {

		for(int i = 0; i < gameControl.Hand.Count; i++) {
			Card card = gameControl.Hand[i].GetComponent<Card>();
			if(card.CardName == "Nunchucks") {
				gameControl.AddPlays(1);
				break;
			}
		}

		base.TargetSquareCalledThis (x, y);
	}
}
