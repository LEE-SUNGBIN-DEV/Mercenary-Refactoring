using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonRightClaw : EnemySkill
{
    [SerializeField] private EnemyCombatController rightClaw;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        cooldown = 8f;
        maxRange = 8f;
        rightClaw = Functions.FindChild<EnemyCombatController>(gameObject, "Right Claw Controller", true);
        rightClaw.Initialize(owner);
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        Owner.Animator.SetTrigger("doRightClaw");
        StartCoroutine(OnRightClaw());
    }
    public IEnumerator OnRightClaw()
    {
        yield return new WaitUntil(() => owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.392f);
        rightClaw.SetCombatController(COMBAT_TYPE.EnemyCompetableAttack, 2f);
        rightClaw.CombatCollider.enabled = true;

        yield return new WaitUntil(() => owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.412f);
        rightClaw.CombatCollider.enabled = false;
    }
}
