using UnityEngine;
using System.Collections;

public class SodaMachine : Card {
	
	public override void Initialize ()
	{
		CardName = "Soda Machine";
 
		base.Initialize ();
	}
	
	public override void Play () {
		battleBoss.Deck.Add ("Soda Cup");

		base.Play ();
	}
}
