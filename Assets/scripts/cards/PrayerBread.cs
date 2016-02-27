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
		S.GameControlInst.AddDollars (1);
		S.GameControlInst.Draw ();

		base.Play ();
	}
}
