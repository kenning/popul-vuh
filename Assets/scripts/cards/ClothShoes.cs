using UnityEngine;
using System.Collections;

public class ClothShoes : Card {
	
	public override void Initialize ()
	{
		CardName = "Cloth Shoes";
		
		base.Initialize ();
	}
	
	public override void Play () {
		battleBoss.AddMoves (2);

        if (Tutorial.TutorialLevel != 0)
        {
            Tutorial.PlayedACardLevel7 = true;
            battleBoss.gameObject.GetComponent<Tutorial>().TutorialMessage = "That's one of Ikka's cards. It lets you move two more times in a turn. Great for running away, I guess.";
        }

		base.Play ();
	}
}
