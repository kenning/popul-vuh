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
		battleBoss.AddMoves (3);
		battleBoss.AddPlays (1);
		
		base.Play ();
	}
}
