using UnityEngine;
using System.Collections;

public class Fireball : Card {

	public override void Initialize ()
	{
		CardName = "Fireball";

		base.Initialize ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}
}
