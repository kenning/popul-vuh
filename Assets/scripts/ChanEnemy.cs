using UnityEngine;
using System.Collections;

public class ChanEnemy : Enemy {
	
	public override void AttackConnects() {
		AnimateAttack ();
		Invoke ("DealDefaultDamage", .3f);
		Invoke ("SetSickBleeding", .3f);
	}
}

