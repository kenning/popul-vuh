using UnityEngine;
using System.Collections;

public class Pizza : Card {
	
	public override void Initialize ()
	{
		CardName = "Pizza";
		
		base.Initialize ();
	}
	
	public override void Play () {

		gameControl.AddPlays (2);

		for(int i = 0; i < gameControl.Hand.Count; i++) {
			if(gameControl.Hand[i].GetComponent<Card>().God == ShopControl.Gods.Chac) {
				break;
			}
			gameControl.Draw();
			gameControl.Draw();
		}
			
		base.Play ();
	}
}
