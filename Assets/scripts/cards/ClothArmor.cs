using UnityEngine;
using System.Collections;

public class ClothArmor : Card {

	public override void Initialize ()
	{
		CardName = "Cloth Armor";

		DamageProtection = 1;
		DamageDistanceProtectionMin = 1;
		DamageDistanceProtectionMax = 8;
		ArmorHasBeenUsed = false;
		
		base.Initialize ();
	}
}
