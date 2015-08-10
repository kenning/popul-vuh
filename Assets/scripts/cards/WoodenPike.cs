using UnityEngine;
using System.Collections;

public class WoodenPike : Card {

	public override void Initialize ()
	{
		CardName = "Wooden Pike";

		base.Initialize ();
	}

    public override void Activate()
    {
        if (Tutorial.TutorialLevel != 0)
        {
            gameControl.gameObject.GetComponent<Tutorial>().TutorialMessage = "Now that it's selected, click on the square with the enemy.";
        }

        base.Activate();
    }

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
	}
}
