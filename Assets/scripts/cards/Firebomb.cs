using UnityEngine;
using System.Collections;

public class Firebomb : Card {

	public override void Initialize ()
	{
		CardName = "Firebomb";
		TitleFontSize = 30;
		BurnsSelfWhenPlayed = true;

		base.Initialize ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 2);
	}
}
