using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKnightHorizontalSlash : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack sword;
    private AnimationClipInfo animationClipInformation;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Horizontal_Slash";
        cooldown = 6;
        minAttackDistance = 0f;
        maxAttackDistance = 4f;

        sword = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Sword", true);
        sword.SetMeleeAttack(enemy);

        animationClipInformation = enemy.AnimationClipTable["Skill_Horizontal_Slash"];
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.Play(animationClipInformation.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, 32));
        sword.SetCombatController(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1f);
        sword.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, 40));
        sword.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, animationClipInformation.maxFrame));
        EndSkill();
    }

    public override IEnumerator CoLookTarget()
    {
        while (!enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, 20))
        {
            enemy.LookTarget();
            yield return null;
        }
    }
}
