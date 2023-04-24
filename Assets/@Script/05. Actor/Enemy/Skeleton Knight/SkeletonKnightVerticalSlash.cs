using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKnightVerticalSlash : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack sword;
    private AnimationClipInformation verticalSlashAnimationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Vertical_Slash";
        cooldown = 7;
        minAttackDistance = 0f;
        maxAttackDistance = 4f;

        sword = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Sword", true);
        sword.SetMeleeAttack(enemy);

        verticalSlashAnimationInfo = enemy.AnimationClipTable["Skill_Vertical_Slash"];
    }

    public override IEnumerator StartSkill()
    {
        enemy.Animator.Play(verticalSlashAnimationInfo.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(verticalSlashAnimationInfo, 32));
        sword.SetCombatController(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1.2f);
        sword.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(verticalSlashAnimationInfo, 40));
        sword.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(verticalSlashAnimationInfo, verticalSlashAnimationInfo.maxFrame));
        EndSkill();
    }
}
