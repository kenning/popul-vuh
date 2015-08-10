using UnityEngine;
using System.Collections;

public class DonaldsXCappuccino : Card {
	
	public override void Initialize ()
	{
		CardName = "Donald's X-Cappuccino";

		base.Initialize ();
	}

	public override void Play () {
		string[] options = new string[] {"+2 plays", "Tuck all cards in your hand and deck, then draw 5"};
		optionControl.SetOptions (options, this);

		base.Play ();
	}

	public override void OptionsCalledThis(int choice) {
		if(choice == 0) {
			gameControl.AddPlays(2);
		}
		else {
			GameObject[] Cards = GameObject.FindGameObjectsWithTag("Card");
			for(int i = 0; i < Cards.Length; i++) {
				Card c = Cards[i].GetComponent<Card>();
				c.Tuck();
			}
			gameControl.Draw();
			gameControl.Draw();
			gameControl.Draw();
			gameControl.Draw();
			gameControl.Draw();
		}

		base.OptionsCalledThis (choice);
	}
}
