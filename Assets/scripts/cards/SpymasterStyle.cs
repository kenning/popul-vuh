using UnityEngine;
using System.Collections;

public class SpymasterStyle : Card {
	
	public override void Initialize ()
	{
		CardName = "Spymaster Style";
		TitleFontSize = 35;

		TriggerResetsOnNewTurn = false;

		base.Initialize ();
	}
	
	public override void Play () {

		EventControl.AddToTriggerList (this);

		base.Play ();
	}
	
	public override void EventCall(string s) {
		if(s == "Enemy Damage") {
			gameControl.Draw();

            eventGUIBoss.AddGUIString("Draw 1!");
		}		
	}
}
