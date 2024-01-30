using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonLeftClaw : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack leftClaw;
    private AnimationClipInfo animationClipInformation;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Left_Claw";
        cooldown = 8f;
        minAttackDistance = 0f;
        maxAttackDistance = 8f;

        leftClaw = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Left Claw Controller", true);
        leftClaw.SetMeleeAttack(enemy);

        animationClipInformation = enemy.AnimationClipTable["Skill_Left_Claw"];
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.Play(animationClipInformation.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, 32));
        leftClaw.SetCombatController(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1f);
        leftClaw.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, 40));
        leftClaw.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, animationClipInformation.maxFrame));
        EndSkill();
    }

    public override IEnumerator CoLookTarget()
    {
        while (!enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, 24))
        {
            enemy.LookTarget();
            yield return null;
        }
    }
}
