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
        gameControl.AddDollars(1);

        if (Tutorial.TutorialLevel != 0)
        {
            Tutorial.PlayedACardLevel7 = true;
            gameControl.gameObject.GetComponent<Tutorial>().TutorialMessage = "That's Ekcha's card, Quick Prayer. You make a prayer to us Gods, which gives you " + 
                "more favor with us. Then you can use it to buy more cards.\nYour favor is in the lower left. It's now $1.";
        }

		base.Play ();
	}
}
