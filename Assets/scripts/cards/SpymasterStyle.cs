using UnityEngine;
using System.Collections;

public class SpymasterStyle : Card {
	
	public override void Initialize ()
	{
		CardName = "Spymaster Style";

		TriggerResetsOnNewTurn = false;

		base.Initialize ();
	}
	
	public override void Play () {

		AddToTriggerList();

		base.Play ();
	}
	
	public override void EventCall(string s) {
		if(s == "Enemy Damage") {
			gameControl.Draw();

            eventGUIControl.AddGUIString("Draw 1!");
		}		
	}
}
