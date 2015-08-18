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


		bool noChacs = true;
		for(int i = 0; i < gameControl.Hand.Count; i++) {
			if(gameControl.Hand[i].GetComponent<Card>().God == ShopControl.Gods.Chac) {
				break;
			}
		}
		if (noChacs) {
			gameControl.Draw();
			gameControl.Draw();
		}
			
		base.Play ();
	}
}
