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

		if (battleBoss.Dollars == 0)
						battleBoss.AddDollars (5);
		else battleBoss.AddDollars(1);

		base.Play ();
	}
}
