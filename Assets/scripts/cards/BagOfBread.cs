﻿using UnityEngine;
using System.Collections;

public class BagOfBread : Card {
	
	public override void Initialize ()
	{
		CardName = "Bag Of Bread";

		DiscardWhenPlayed = false;

		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.Draw ();
		gameControl.Draw ();

		base.Play ();
	}
}
