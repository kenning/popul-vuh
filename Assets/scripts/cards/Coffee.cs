using UnityEngine;
using System.Collections;

public class Coffee : Card {
	
	public override void Initialize ()
	{
		CardName = "Coffee";
		
		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.AddPlays (2);

        if (Tutorial.TutorialLevel != 0)
        {
            Tutorial.PlayedACardLevel7 = true;
            gameControl.gameObject.GetComponent<Tutorial>().TutorialMessage = "Coffee is actually pretty useful, because it lets you use two of my cards in a turn. Or any other two cards.";
        }

		base.Play ();
	}
}
