using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChocolateSurpriseEgg : Card {

	//public List<string> cardOptions;
	
	public override void Initialize ()
	{
		CardName = "Chocolate Surprise Egg";

		base.Initialize ();
	}
	
	public override void Play () {

		gameControl.Draw ();
		gameControl.AddPlays (1);
		gameControl.AddDollars (1);

		base.Play ();
	}

	public override void Burn () {
		gameControl.Deck.Add ("Phoenix Fireball");

		base.Burn ();
	}
}
