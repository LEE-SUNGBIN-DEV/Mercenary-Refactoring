using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonLeftClaw : EnemySkill
{
    public override void Initialize(Enemy owner)
    {
        base.Initialize(owner);
        cooldown = 8f;
        maxRange = 8f;
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        Owner.Animator.SetTrigger("doLeftClawAttack");
    }
}
