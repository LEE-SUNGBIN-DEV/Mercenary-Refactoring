using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonLeftClaw : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack leftClaw;
    private AnimationClipInformation leftClawAnimationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Left_Claw";
        cooldown = 8f;
        minAttackDistance = 0f;
        maxAttackDistance = 8f;

        leftClaw = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Left Claw Controller", true);
        leftClaw.SetMeleeAttack(enemy);

        leftClawAnimationInfo = enemy.AnimationClipTable["Skill_Left_Claw"];
    }

    public override IEnumerator StartSkill()
    {
        enemy.Animator.Play(leftClawAnimationInfo.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(leftClawAnimationInfo, 32));
        leftClaw.SetCombatController(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1f);
        leftClaw.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(leftClawAnimationInfo, 40));
        leftClaw.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(leftClawAnimationInfo, leftClawAnimationInfo.maxFrame));
        EndSkill();
    }
}
