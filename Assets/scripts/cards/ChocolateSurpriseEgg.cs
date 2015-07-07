using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChocolateSurpriseEgg : Card {

	//public List<string> cardOptions;
	
	public override void Initialize ()
	{
		CardName = "Chocolate Surprise Egg";
		TitleFontSize = 30;
		SmallFontSize = 30;

		base.Initialize ();
	}
	
	public override void Play () {

		battleBoss.Draw ();
		battleBoss.AddPlays (1);
		battleBoss.AddDollars (1);

		base.Play ();
	}

	public override void Burn () {
		battleBoss.Deck.Add ("Phoenix Fireball");

		base.Burn ();
	}
}
