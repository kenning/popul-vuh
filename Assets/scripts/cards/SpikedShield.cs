using UnityEngine;
using System.Collections;

public class SpikedShield : Card {

	public override void Initialize ()
	{
		CardName = "Spiked Shield";

		DamageProtection = 1;
		DamageDistanceProtectionMin = 2;
		DamageDistanceProtectionMax = 3;
		ArmorHasBeenUsed = false;
		ArmorWithSecondaryAbility = true;
		
		base.Initialize ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 2);
	}
}
