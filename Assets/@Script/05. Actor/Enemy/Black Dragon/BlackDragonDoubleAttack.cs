using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonDoubleAttack : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack leftClaw;
    [SerializeField] private EnemyMeleeAttack rightClaw;
    private AnimationClipInformation doubleClawAnimationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Double_Claw";
        cooldown = 16f;
        minAttackDistance = 0f;
        maxAttackDistance = 8f;

        leftClaw = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Left Claw Controller", true);
        rightClaw = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Right Claw Controller", true);
        leftClaw.SetMeleeAttack(enemy);
        rightClaw.SetMeleeAttack(enemy);

        doubleClawAnimationInfo = enemy.AnimationClipTable["Skill_Double_Claw"];
    }

    public override IEnumerator StartSkill()
    {
        enemy.Animator.Play(doubleClawAnimationInfo.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(doubleClawAnimationInfo, 32));
        leftClaw.SetCombatController(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1f);
        leftClaw.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(doubleClawAnimationInfo, 40));
        leftClaw.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(doubleClawAnimationInfo, 127));

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(doubleClawAnimationInfo, 140));
        rightClaw.SetCombatController(HIT_TYPE.STUN, GUARD_TYPE.NONE, 1.2f, 4f);
        rightClaw.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(doubleClawAnimationInfo, 142));
        rightClaw.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(doubleClawAnimationInfo, doubleClawAnimationInfo.maxFrame));
        EndSkill();
    }
}