using UnityEngine;
using System.Collections;

public class TigerStance : Card {
	
	public override void Initialize ()
	{
		CardName = "Tiger Stance";
		SmallFontSize = 35;

		base.Initialize ();
	}
	
	public override void Play () {

		battleBoss.AddMoves (1);
		battleBoss.AddPlays (1);
		EventControl.AddToTriggerList (this);

		base.Play ();
	}
	
	public override void EventCall(string s) {
		if(s == "Enemy Death") {
			battleBoss.AddMoves(1);
			battleBoss.AddPlays(1);

            eventGUIBoss.AddGUIString("Move +1! Play +1!");
		}		
	}
}
