using UnityEngine;
using System.Collections;

public class DesperatePrayer : Card {
	
	public override void Initialize ()
	{
		CardName = "Desperate Prayer";

		base.Initialize ();
	}
	
	public override void Play () {

		if (S.GameControlInst.Dollars == 0)
						S.GameControlInst.AddDollars (5);
		else S.GameControlInst.AddDollars(1);

		base.Play ();
	}
}
