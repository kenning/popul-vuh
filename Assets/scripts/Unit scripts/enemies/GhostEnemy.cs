using UnityEngine;
using System.Collections;

public class GhostEnemy : Enemy {
	
	public override void AttackConnects() {
		AnimateAttack ();
		Invoke ("ForceDiscard", .3f);
	}
}

