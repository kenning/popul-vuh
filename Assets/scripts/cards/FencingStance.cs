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

		S.GameControlInst.AddMoves (2);
		S.GameControlInst.AddPlays (1);
		AddToTriggerList();

		base.Play ();
	}
	
	public override void EventCall(string s) {
		if(s == "Punch") {
			S.GameControlInst.AddMoves(2);
			eventGUIControl.AddGUIString("Move +2!");
		}
	}
}
