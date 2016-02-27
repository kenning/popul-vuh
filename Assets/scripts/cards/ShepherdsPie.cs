using UnityEngine;
using System.Collections;

public class ShepherdsPie : Card {
	
	public override void Initialize ()
	{
		CardName = "Shepherd's Pie";

		base.Initialize ();
	}
	
	public override void Play ()
	{
		S.GameControlInst.Draw();
		S.GameControlInst.Draw();

		base.Play ();
	}
}
