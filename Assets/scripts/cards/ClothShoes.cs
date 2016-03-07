using UnityEngine;
using System.Collections;

public class ClothShoes : Card {
	
	public override void Initialize ()
	{
		CardName = "Cloth Shoes";
		
		base.Initialize ();
	}
	
	public override void Play () {
		S.GameControlInst.AddMoves (2);

        if (Tutorial.TutorialLevel != 0)
        {
            Tutorial.PlayedACardLevel6 = true;
            S.TutorialInst.TurnOffArrows();
            S.GameControlInst.gameObject.GetComponent<Tutorial>().TutorialMessage = "ghojinmpk loipnmjk";
        }

		base.Play ();
	}
}
