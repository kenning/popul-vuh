using UnityEngine;
using System.Collections;

public class Apple : Card {
	
	public override void Initialize ()
	{
		CardName = "Apple";

		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.Draw ();
		gameControl.Draw ();
		gameControl.AddPlays (1);

        if (Tutorial.TutorialLevel != 0)
        {
            Tutorial.PlayedACardLevel7 = true;
            gameControl.gameObject.GetComponent<Tutorial>().TutorialMessage = "That's Chac's card, Apple. It draws you two cards and lets you play another card.";
        }

		base.Play ();
	}
}
