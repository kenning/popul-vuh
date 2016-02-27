using UnityEngine;
using System.Collections;

public class NagualJaguar : Enemy {
    public override void AttackConnects()
    {
        AnimateAttack();
        //THIS SHIT BROKE
        //S.GameControlInst.Invoke("TransformPlayer", .3f);
    }
}
