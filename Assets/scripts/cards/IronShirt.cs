using UnityEngine;
using System.Collections;

public class IronShirt : Card {

	public override void Initialize ()
	{
		CardName = "Iron Shirt";
		DamageProtection = 2;
		DamageDistanceProtectionMin = 1;
		DamageDistanceProtectionMax = 1;
		ArmorHasBeenUsed = false;
		
		base.Initialize ();
	}
}
