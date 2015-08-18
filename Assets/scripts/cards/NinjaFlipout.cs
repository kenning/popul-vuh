using UnityEngine;
using System.Collections;

public class NinjaFlipout : Card {

	public override void Initialize ()
	{
		CardName = "Ninja Flipout";

		base.Initialize ();
	}

	public override void Play ()
	{
		int range = -1;
		for (var i = gameControl.Hand.IndexOf(gameObject); i < gameControl.Hand.Count; i++) {
			range++;
			if(range == 3) break;
		}
		aoeMaxRange = range;
		GridUnit playerGU = playerObj.GetComponent<GridUnit> ();
		FindAndAffectUnits ((int)playerGU.xPosition, (int)playerGU.yPosition);

		base.Play ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}
}
