using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonRightClaw : EnemySkill
{
    [SerializeField] private EnemyCompeteAttack rightClaw;
    private AnimationClipInformation rightClawAnimationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Right_Claw";
        cooldown = 8f;
        minAttackDistance = 0f;
        maxAttackDistance = 8f;

        rightClaw = Functions.FindChild<EnemyCompeteAttack>(gameObject, "Compete Controller", true);
        rightClaw.SetCompeteAttack(enemy);

        rightClawAnimationInfo = enemy.AnimationClipDictionary["Skill_Right_Claw"];
    }

    public override IEnumerator StartSkill()
    {
        enemy.Animator.Play(rightClawAnimationInfo.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(rightClawAnimationInfo, 40));
        rightClaw.SetCombatController(COMBAT_TYPE.ATTACK_STUN, 2f, 3f);
        rightClaw.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(rightClawAnimationInfo, 42));
        rightClaw.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(rightClawAnimationInfo, rightClawAnimationInfo.maxFrame));
        EndSkill();
    }
}
