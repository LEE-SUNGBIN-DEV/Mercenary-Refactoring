using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonRightClaw : EnemySkill
{
    [SerializeField] private EnemyCompeteAttack rightClaw;
    private AnimationClipInformation animationClipInformation;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Right_Claw";
        cooldown = 8f;
        minAttackDistance = 0f;
        maxAttackDistance = 8f;

        rightClaw = Functions.FindChild<EnemyCompeteAttack>(gameObject, "Compete Controller", true);
        rightClaw.SetCompeteAttack(enemy);

        animationClipInformation = enemy.AnimationClipTable["Skill_Right_Claw"];
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.Play(animationClipInformation.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, 40));
        rightClaw.SetCombatController(HIT_TYPE.STUN, GUARD_TYPE.NONE, 2f, 3f);
        rightClaw.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, 42));
        rightClaw.OnDisableCollider();

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
