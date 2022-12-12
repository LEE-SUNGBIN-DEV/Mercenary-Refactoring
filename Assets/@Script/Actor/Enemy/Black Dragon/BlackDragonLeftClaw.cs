using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonLeftClaw : EnemySkill
{
    [SerializeField] private EnemyCombatController leftClaw;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        cooldown = 8f;
        maxRange = 8f;
        leftClaw = Functions.FindChild<EnemyCombatController>(gameObject, "Left Claw Controller", true);
        leftClaw.Initialize(owner);
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        Owner.Animator.SetTrigger("doLeftClaw");
        StartCoroutine(OnLeftClaw());
    }
    public IEnumerator OnLeftClaw()
    {
        yield return new WaitUntil(() => owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.32f);
        leftClaw.SetCombatController(COMBAT_TYPE.EnemyNormalAttack, 1f);
        leftClaw.CombatCollider.enabled = true;

        yield return new WaitUntil(() => owner.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.40f);
        leftClaw.CombatCollider.enabled = false;
    }
}
