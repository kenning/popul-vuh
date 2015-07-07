using UnityEngine;
using System.Collections;

public class SodaCup : Card {
	
	public override void Initialize ()
	{
		CardName = "Soda Cup";
		BurnsSelfWhenPlayed = true;
		
		base.Initialize ();
	}
	
	public override void Play () {
		battleBoss.Draw ();
		battleBoss.Draw ();
		battleBoss.AddPlays (2);
		Invoke ("OrganizeCards", .5f);
		base.Play ();
	}
}
