using UnityEngine;
using System.Collections;

public class FencingStance : Card {
	
	public override void Initialize ()
	{
		CardName = "Fencing Stance";
		TriggerResetsOnNewTurn = true;

		base.Initialize ();
	}
	
	public override void Play () {

		battleBoss.AddMoves (2);
		battleBoss.AddPlays (1);
		EventControl.AddToTriggerList (this);

		base.Play ();
	}
	
	public override void EventCall(string s) {
		if(s == "Punch") {
			battleBoss.AddMoves(2);
            Debug.Log("got there");
			eventGUIBoss.AddGUIString("Move +2!");
		}
	}
}
