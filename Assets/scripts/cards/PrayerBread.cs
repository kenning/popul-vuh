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
		battleBoss.AddDollars (1);
		battleBoss.Draw ();

		base.Play ();
	}
}
