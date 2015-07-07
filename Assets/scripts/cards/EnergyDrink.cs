using UnityEngine;
using System.Collections;

public class EnergyDrink : Card {
	
	public override void Initialize ()
	{
		CardName = "Energy Drink";
		
		base.Initialize ();
	}
	
	public override void Play () {
		battleBoss.AddPlays (3);
		battleBoss.Draw ();

		base.Play ();
	}
}
