using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKnightVerticalSlash : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack sword;
    private AnimationInfo verticalSlashAnimationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Vertical_Slash";
        cooldown = 7;
        minAttackDistance = 0f;
        maxAttackDistance = 4f;

        sword = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Sword", true);
        sword.SetMeleeAttack(enemy);

        verticalSlashAnimationInfo = new AnimationInfo(skillName, 8.417f, 202, 2f);
    }

    public override IEnumerator StartSkill()
    {
        enemy.Animator.Play(verticalSlashAnimationInfo.animationNameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(verticalSlashAnimationInfo, 32));
        sword.SetCombatController(COMBAT_TYPE.ATTACK_HEAVY, 1.2f);
        sword.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(verticalSlashAnimationInfo, 40));
        sword.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(verticalSlashAnimationInfo, verticalSlashAnimationInfo.maxFrame));
        EndSkill();
    }
}
