using UnityEngine;
using System.Collections;

public class IronPike : Card {

	public override void Initialize ()
	{
		CardName = "Iron Pike";

		base.Initialize ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 2);
	}
}
