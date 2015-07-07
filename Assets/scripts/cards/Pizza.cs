using UnityEngine;
using System.Collections;

public class Pizza : Card {
	
	public override void Initialize ()
	{
		CardName = "Pizza";
		
		base.Initialize ();
	}
	
	public override void Play () {

		battleBoss.AddPlays (2);

		for(int i = 0; i < battleBoss.Hand.Count; i++) {
			if(battleBoss.Hand[i].GetComponent<Card>().God == ShopControl.Gods.Chac) {
				break;
			}
			battleBoss.Draw();
			battleBoss.Draw();
		}
			
		base.Play ();
	}
}
