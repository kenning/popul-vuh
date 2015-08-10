using UnityEngine;
using System.Collections;

public class NagualJaguar : Enemy {
    public override void AttackConnects()
    {
        AnimateAttack();
        //THIS SHIT BROKE
        //gameControl.Invoke("TransformPlayer", .3f);
    }
}
