using UnityEngine;
using System.Collections;

public class IronShield : Card {

	public override void Initialize ()
	{
		CardName = "Iron Shield";
		DamageProtection = 2;
		DamageDistanceProtectionMin = 2;
		DamageDistanceProtectionMax = 3;
		ArmorHasBeenUsed = false;
		
		base.Initialize ();
	}
}
