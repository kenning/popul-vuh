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
		S.GameControlInst.AddMoves (3);
		S.GameControlInst.AddPlays (1);
		
		base.Play ();
	}
}
