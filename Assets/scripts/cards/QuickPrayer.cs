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
            Tutorial.PlayedACardLevel7 = true;
            S.GameControlInst.gameObject.GetComponent<Tutorial>().TutorialMessage = "That's Ekcha's card, Quick Prayer. You make a prayer to us Gods, which gives you " + 
                "more favor ($) with us. Then you can use it to buy more cards.";
        }

		base.Play ();
	}
}
