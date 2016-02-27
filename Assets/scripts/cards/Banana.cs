using UnityEngine;
using System.Collections;

public class Banana : Card {
	
	public override void Initialize ()
	{
		CardName = "Banana";

		FreeTargetSquare = true;

		base.Initialize ();
	}
	
	public override void Play () {
		S.GameControlInst.Draw ();

		base.Play ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicStunEffect (targetedUnit);
	}
}
