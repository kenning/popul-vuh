using UnityEngine;
using System.Collections;

public class Pizza : Card {
	
	public override void Initialize ()
	{
		CardName = "Pizza";
		
		base.Initialize ();
	}
	
	public override void Play () {

		S.GameControlInst.AddPlays (2);


		bool noChacs = true;
		for(int i = 0; i < S.GameControlInst.Hand.Count; i++) {
			if(S.GameControlInst.Hand[i].GetComponent<Card>().God == ShopControl.Gods.Chac) {
				break;
			}
		}
		if (noChacs) {
			S.GameControlInst.Draw();
			S.GameControlInst.Draw();
		}
			
		base.Play ();
	}
}
