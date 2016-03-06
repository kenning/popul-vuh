using UnityEngine;
using System.Collections;

public class IronMacana : Card {

	public override void Initialize ()
	{
		CardName = "Iron Macana";

		base.Initialize ();
	}

    public override void Click()
    {
        if (Tutorial.TutorialLevel != 0)
        {
            Select();
            S.GameControlInst.gameObject.GetComponent<Tutorial>().TutorialMessage = "blajpeohgifndcmk hmnlopijk iojkmlnp dojbfghcepknlmi";
        }
        else
        {
            base.Click();
        }
    }

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 2);
	}
}
