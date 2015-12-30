using UnityEngine;
using System.Collections;

public class WoodenBow : Card {

	public override void Initialize ()
	{
		CardName = "Wooden Bow";

		base.Initialize ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}
}
