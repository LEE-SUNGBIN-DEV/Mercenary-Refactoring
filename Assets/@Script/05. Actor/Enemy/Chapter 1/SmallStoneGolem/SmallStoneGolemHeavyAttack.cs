using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallStoneGolemHeavyAttack : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack heavyAttack;
    private AnimationClipInformation animationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Heavy_Attack";
        cooldown = 8f;
        minAttackDistance = 0f;
        maxAttackDistance = 1.5f;

        heavyAttack = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Heavy_Attack_Controller", true);
        heavyAttack.SetMeleeAttack(enemy);

        animationInfo = enemy.AnimationClipDictionary["Skill_Heavy_Attack"];
    }

    public override IEnumerator StartSkill()
    {
        enemy.Animator.Play(animationInfo.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 24));
        heavyAttack.SetCombatController(COMBAT_TYPE.ATTACK_HEAVY, 1.5f);
        heavyAttack.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 25));
        heavyAttack.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, animationInfo.maxFrame));
        EndSkill();
    }
}
