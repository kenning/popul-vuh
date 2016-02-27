﻿using UnityEngine;
using System.Collections;

public class DeepPrayer : Card {
	
	public override void Initialize ()
	{
		CardName = "Deep Prayer";

		base.Initialize ();
	}
	
	public override void Play ()
	{
		S.GameControlInst.AddDollars (2);

		base.Play ();
	}
}
