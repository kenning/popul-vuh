using UnityEngine;
using System.Collections;

public class BirthdayCake : Card {
	
	public override void Initialize ()
	{
		CardName = "Birthday Cake";

		base.Initialize ();
	}

	public override void Play ()
	{
		S.GameControlInst.Draw ();
		S.GameControlInst.Draw ();
		S.GameControlInst.Draw ();

		base.Play ();
	}

	public override void Burn() {
		S.GameControlInst.AddDollars (10);
		base.Burn ();
	}
}
