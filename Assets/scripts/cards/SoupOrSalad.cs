using UnityEngine;
using System.Collections;

public class SoupOrSalad : Card {
	
	public override void Initialize ()
	{
		CardName = "Soup Or Salad";

		base.Initialize ();
	}

	public override void Play () {
		string[] options = new string[] {"+2 cards", "+2 plays"};
		optionBoss.SetOptions (options, this);

		base.Play ();
	}

	public override void OptionsCalledThis(int choice) {
		if(choice == 0) {
			battleBoss.Draw();
			battleBoss.Draw();
		}
		else {
			battleBoss.AddPlays(2);
		}

		Invoke ("OrganizeCards", 1f);
		base.OptionsCalledThis (choice);
	}
}
