using UnityEngine;
using System.Collections;

public class Coffee : Card {
	
	public override void Initialize ()
	{
		CardName = "Coffee";
		
		base.Initialize ();
	}
	
	public override void Play () {
		S.GameControlInst.AddPlays (2);

        if (Tutorial.TutorialLevel != 0)
        {
            Tutorial.PlayedACardLevel6 = true;
            S.TutorialInst.TurnOffArrows();
            S.GameControlInst.gameObject.GetComponent<Tutorial>().TutorialMessage = "kojpnlm nmopkijlfh";
        }

		base.Play ();
	}
}
