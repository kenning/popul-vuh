using UnityEngine;
using System.Collections;

public class PrayerBeads : Card {
	
	public override void Initialize ()
	{
		CardName = "Prayer Beads";
		base.Initialize ();
	}
	
	public override void Play () {

		gameControl.AddPlays (1);
		AddToTriggerList();

		base.Play ();
	}
	
	public override void EventCall(string s) {
		if(s == "Burn") {
			gameControl.AddDollars(3);

            eventGUIControl.AddGUIString("+$3!");
		}
	}
}
