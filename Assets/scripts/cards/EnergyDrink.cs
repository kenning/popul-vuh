using UnityEngine;
using System.Collections;

public class EnergyDrink : Card {
	
	public override void Initialize ()
	{
		CardName = "Energy Drink";
		
		base.Initialize ();
	}
	
	public override void Play () {
		S.GameControlInst.AddPlays (3);
		S.GameControlInst.Draw ();

		base.Play ();
	}
}
