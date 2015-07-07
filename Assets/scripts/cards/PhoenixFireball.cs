using UnityEngine;
using System.Collections;

public class PhoenixFireball : Card {

	public override void Initialize ()
	{
		CardName = "Phoenix Fireball";

		base.Initialize ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}
}
