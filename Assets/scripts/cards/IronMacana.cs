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
            gameControl.gameObject.GetComponent<Tutorial>().TutorialMessage = "You tapped the wrong card. It's OK, I know they look the same. Select the pike and then tap on the enemy.";
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
