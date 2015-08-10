using UnityEngine;
using System.Collections;

public class EnergyDrink : Card {
	
	public override void Initialize ()
	{
		CardName = "Energy Drink";
		
		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.AddPlays (3);
		gameControl.Draw ();

		base.Play ();
	}
}
