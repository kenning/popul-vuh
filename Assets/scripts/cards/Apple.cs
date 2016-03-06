using UnityEngine;
using System.Collections;

public class Apple : Card {
	
	public override void Initialize ()
	{
		CardName = "Apple";

		base.Initialize ();
	}
	
	public override void Play () {
		S.GameControlInst.Draw ();
		S.GameControlInst.Draw ();
		S.GameControlInst.AddPlays (1);

        if (Tutorial.TutorialLevel != 0)
        {
            Tutorial.PlayedACardLevel6 = true;
            S.GameControlInst.gameObject.GetComponent<Tutorial>().TutorialMessage = "fnjlhmpogik onhpmklefijg lpkmonj kpnliomj";
        }

		base.Play ();
	}
}
