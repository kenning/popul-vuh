using UnityEngine;
using System.Collections;

public class PrayerBeads : Card {
	
	public override void Initialize ()
	{
		CardName = "Prayer Beads";
		base.Initialize ();
	}
	
	public override void Play () {

		battleBoss.AddPlays (1);
		EventControl.AddToTriggerList (this);

		base.Play ();
	}
	
	public override void EventCall(string s) {
		if(s == "Burn") {
			battleBoss.AddDollars(3);

            eventGUIBoss.AddGUIString("+$3!");
		}
	}
}
