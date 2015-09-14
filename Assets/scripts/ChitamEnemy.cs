using UnityEngine;
using System.Collections;

public class ChitamEnemy : Enemy {
	
	public override void AttackConnects() {
		AnimateAttack ();
		Invoke ("DealDefaultDamage", .3f);
		Invoke ("ForceDiscard", .3f);
	}
}

