using UnityEngine;
using System.Collections;

public class WoodShield : Card {

	public override void Initialize ()
	{
		CardName = "Wood Shield";
		DamageProtection = 1;
		DamageDistanceProtectionMin = 2;
		DamageDistanceProtectionMax = 3;
		ArmorHasBeenUsed = false;
		
		base.Initialize ();
	}
}
