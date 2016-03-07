using UnityEngine;
using System.Collections;

public class WoodenPike : Card {

	public override void Initialize ()
	{
		CardName = "Wooden Pike";

		base.Initialize ();
	}

    public override void Activate(bool freeMove)
    {
        if (Tutorial.TutorialLevel != 0)
        {
            S.GameControlInst.gameObject.GetComponent<Tutorial>().TutorialMessage = "inlmjph opjmnilk lkopjmn";
            S.TutorialInst.TurnOffArrows();
            S.TutorialInst.MakeArrowInSpot(0, -2);
        }
        
        base.Activate(freeMove);
    }

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, 1);
        
        Tutorial.PlayedACardLevel5 = true;
        S.TutorialInst.TurnOffArrows();
	}
}
