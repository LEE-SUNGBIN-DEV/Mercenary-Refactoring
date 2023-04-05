using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallStoneGolemHeavyAttack : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack heavyAttack;
    private AnimationInfo animationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Heavy_Attack";
        cooldown = 8f;
        minAttackDistance = 0f;
        maxAttackDistance = 1.5f;

        heavyAttack = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Heavy_Attack_Controller", true);
        heavyAttack.SetMeleeAttack(enemy);

        animationInfo = new AnimationInfo(skillName, 1.875f, 45, 1f);
    }

    public override IEnumerator StartSkill()
    {
        enemy.Animator.Play(animationInfo.animationNameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 24));
        heavyAttack.SetCombatController(COMBAT_TYPE.ATTACK_HEAVY, 1.5f);
        heavyAttack.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 25));
        heavyAttack.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, animationInfo.maxFrame));
        EndSkill();
    }
}
