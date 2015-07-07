using UnityEngine;
using System.Collections;

public class NagualJaguar : Enemy {
    public override void AttackConnects()
    {
        AnimateAttack();
        //THIS SHIT BROKE
        //battleBoss.Invoke("TransformPlayer", .3f);
    }
}
