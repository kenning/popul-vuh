using UnityEngine;
using System.Collections;

public class Boomerang : Card {

	public override void Initialize ()
	{
		CardName = "Boomerang";

		DiscardWhenPlayed = false;

		base.Initialize ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 2);
	}
}
