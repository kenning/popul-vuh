using UnityEngine;
using System.Collections;

public class SpinKick : Card {

	public override void Initialize ()
	{
		CardName = "Spin Kick";

		base.Initialize ();
	}

	public override void Play ()
	{
		GridUnit playerGU = playerObj.GetComponent<GridUnit> ();
		FindAndAffectUnits ((int)playerGU.xPosition, (int)playerGU.yPosition);

		base.Play ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}
}
