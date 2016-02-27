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

		S.GameControlInst.Draw ();
		S.GameControlInst.AddPlays (1);
		S.GameControlInst.AddDollars (1);

		base.Play ();
	}

	public override void Burn () {
		S.GameControlInst.Deck.Add ("Phoenix Fireball");

		base.Burn ();
	}
}
