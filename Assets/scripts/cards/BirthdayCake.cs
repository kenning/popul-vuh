using UnityEngine;
using System.Collections;

public class BirthdayCake : Card {
	
	public override void Initialize ()
	{
		CardName = "Birthday Cake";
		TitleFontSize = 40;
		base.Initialize ();
	}

	public override void Play ()
	{
		gameControl.Draw ();
		gameControl.Draw ();
		gameControl.Draw ();

		base.Play ();
	}

	public override void Burn() {
		gameControl.AddDollars (10);
		base.Burn ();
	}
}
