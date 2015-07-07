using UnityEngine;
using System.Collections;

public class MagicMissile : Card {
	
	public override void Initialize ()
	{
		CardName = "Magic Missile";

		base.Initialize ();
	}
	
	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}
}
