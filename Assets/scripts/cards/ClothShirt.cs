using UnityEngine;
using System.Collections;

public class ClothShirt : Card {
	
	public override void Initialize ()
	{
		CardName = "Cloth Shirt";
		DamageProtection = 1;
		DamageDistanceProtectionMin = 1;
		DamageDistanceProtectionMax = 1;
		ArmorHasBeenUsed = false;
		
		base.Initialize ();
	}
}
