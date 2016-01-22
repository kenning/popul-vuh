using UnityEngine;
using System.Collections;

public class TigerStance : Card {
	
	public override void Initialize ()
	{
		CardName = "Tiger Stance";

		base.Initialize ();
	}
	
	public override void Play () {

		gameControl.AddMoves (1);
		gameControl.AddPlays (1);
		AddToTriggerList();

		base.Play ();
	}
	
	public override void EventCall(string s) {
		if(s == "Enemy Death") {
			gameControl.AddMoves(1);
			gameControl.AddPlays(1);

			eventGUIControl.AddGUIString("Move +1! Play +1!");
		}		
	}
}
