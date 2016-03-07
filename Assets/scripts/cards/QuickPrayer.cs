using UnityEngine;
using System.Collections;

public class QuickPrayer : Card {
	
	public override void Initialize ()
	{
		CardName = "Quick Prayer";

		base.Initialize ();
	}
	
	public override void Play ()
	{
        S.GameControlInst.AddDollars(1);

        if (Tutorial.TutorialLevel != 0)
        {
            Tutorial.PlayedACardLevel6 = true;
            S.TutorialInst.TurnOffArrows();
            S.GameControlInst.gameObject.GetComponent<Tutorial>().TutorialMessage = "jmlknpo ojpnlm";
        }

		base.Play ();
	}
}
