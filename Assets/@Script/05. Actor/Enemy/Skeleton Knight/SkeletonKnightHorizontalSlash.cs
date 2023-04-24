using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKnightHorizontalSlash : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack sword;
    private AnimationClipInformation horizontalSlashAnimationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Horizontal_Slash";
        cooldown = 6;
        minAttackDistance = 0f;
        maxAttackDistance = 4f;

        sword = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Sword", true);
        sword.SetMeleeAttack(enemy);

        horizontalSlashAnimationInfo = enemy.AnimationClipTable["Skill_Horizontal_Slash"];
    }

    public override IEnumerator StartSkill()
    {
        enemy.Animator.Play(horizontalSlashAnimationInfo.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(horizontalSlashAnimationInfo, 32));
        sword.SetCombatController(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1f);
        sword.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(horizontalSlashAnimationInfo, 40));
        sword.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(horizontalSlashAnimationInfo, horizontalSlashAnimationInfo.maxFrame));
        EndSkill();
    }
}
