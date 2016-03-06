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
            S.GameControlInst.gameObject.GetComponent<Tutorial>().TutorialMessage = "oilkhjpnm lkjhogemnifp kpnlfhjgiom ghojinmlfpk loipnmjk";
        }

		base.Play ();
	}
}
