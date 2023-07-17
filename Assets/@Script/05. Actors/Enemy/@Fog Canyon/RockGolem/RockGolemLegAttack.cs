using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGolemLegAttack : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack legAttack;
    private AnimationClipInformation animationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Leg_Attack";
        cooldown = 5f;
        minAttackDistance = 0f;
        maxAttackDistance = 2.2f;

        legAttack = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Leg_Attack_Controller", true);
        legAttack.SetMeleeAttack(enemy);

        animationInfo = enemy.AnimationClipTable["Skill_Leg_Attack"];
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.CrossFadeInFixedTime(animationInfo.nameHash, 0.2f);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 38));
        legAttack.SetCombatController(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1f);
        legAttack.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 42));
        legAttack.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, animationInfo.maxFrame));
        EndSkill();
    }

    public override IEnumerator CoLookTarget()
    {
        while (!enemy.Animator.IsAnimationFrameUpTo(animationInfo, 30))
        {
            enemy.LookTarget();
            yield return null;
        }
    }
}
