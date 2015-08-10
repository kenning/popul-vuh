using UnityEngine;
using System.Collections;

public class SpeedBurst : Card {
	
	public override void Initialize ()
	{
		CardName = "Speed Burst";
		
		base.Initialize ();
	}
	
	public override void Play ()
	{
		gameControl.AddMoves (3);
		gameControl.AddPlays (1);
		
		base.Play ();
	}
}
