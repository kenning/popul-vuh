using UnityEngine;
using System.Collections;

public class BrokenBeerBottle : Card {
	
	public override void Initialize ()
	{
		CardName = "Broken Beer Bottle";
	
		base.Initialize ();
	}
	
	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}

	public override void Tuck() {
		cardUI.MoveAnimate (gameControl.Hand.Count);
	}
}
