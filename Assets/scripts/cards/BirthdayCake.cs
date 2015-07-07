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
		battleBoss.Draw ();
		battleBoss.Draw ();
		battleBoss.Draw ();

		base.Play ();
	}

	public override void Burn() {
		battleBoss.AddDollars (10);
		base.Burn ();
	}
}
