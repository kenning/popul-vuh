using UnityEngine;
using System.Collections;

public class PrayerBread : Card {
	
	public override void Initialize ()
	{
		CardName = "Prayer Bread";

		base.Initialize ();
	}
	
	public override void Play ()
	{
		gameControl.AddDollars (1);
		gameControl.Draw ();

		base.Play ();
	}
}
