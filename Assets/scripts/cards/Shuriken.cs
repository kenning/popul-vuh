using UnityEngine;
using System.Collections;

public class Shuriken : Card {

	int x;

	public override void Initialize ()
	{
		CardName = "Shuriken";

		FreeTargetSquare = true;

		base.Initialize ();
	}
	
	public override void Play () {
		gameControl.CardsToTarget = 1;

		gameControlGUI.Dim(true);
		gameControl.Tooltip = "Pick a card to discard.";
	}
	
	public override void AfterCardTargetingCallback() {

		Card.Rarity discardRarity = Rarity.Paper;

		foreach(GameObject tempGO in gameControl.TargetedCards){
			Card c = tempGO.GetComponent<Card>();
			discardRarity = c.ThisRarity;
			c.Discard();
		}

		if (discardRarity == Rarity.Paper)
			x = 1;
		if (discardRarity == Rarity.Bronze)
			x = 2;
		if (discardRarity == Rarity.Silver)
			x = 3;
		if (discardRarity == Rarity.Gold)
			x = 4;
		if (discardRarity == Rarity.Platinum)
			x = 5;

		gridControl.EnterTargetingMode(GridControl.TargetTypes.diamond, 1, x);

		//these must be done manually because after targeting a card, you must finish the card action by attacking.
		clickControl.DisallowEveryInput ();
		clickControl.AllowInfoInput = true;
		clickControl.AllowSquareTargetInput = true;
		gameControl.TargetSquareCallback = this;

		QControl.AddToQ (this, QControl.QMethodType.FreeSpecial);

		base.AfterCardTargetingCallback ();
	}

	public override void SpecialQCall ()
	{
		Debug.Log ("got to special mode");
		minRange = 1;
		maxRange = x;
		rangeTargetType = GridControl.TargetTypes.diamond;
		EnterTargetingMode ();
	}

	public override void Affect (GridUnit targetedUnit) {
		BasicDamageEffect (targetedUnit, x);
	}
}
