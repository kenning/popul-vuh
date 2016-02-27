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
            Tutorial.PlayedACardLevel7 = true;
            S.GameControlInst.gameObject.GetComponent<Tutorial>().TutorialMessage = "That's Chac's card, Apple. It draws you two cards and lets you play another card.";
        }

		base.Play ();
	}
}
