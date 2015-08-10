using UnityEngine;
using System.Collections;

public class DesperatePrayer : Card {
	
	public override void Initialize ()
	{
		CardName = "Desperate Prayer";
		TitleFontSize = 35;

		base.Initialize ();
	}
	
	public override void Play () {

		if (gameControl.Dollars == 0)
						gameControl.AddDollars (5);
		else gameControl.AddDollars(1);

		base.Play ();
	}
}
