using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallStoneGolemLightAttack : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack lightAttack;
    private AnimationInfo animationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Light_Attack";
        cooldown = 5f;
        minAttackDistance = 0f;
        maxAttackDistance = 1f;

        lightAttack = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Light_Attack_Controller", true);
        lightAttack.SetMeleeAttack(enemy);

        animationInfo = new AnimationInfo(skillName, 1.667f, 40, 1f);
    }

    public override IEnumerator StartSkill()
    {
        enemy.Animator.Play(animationInfo.animationNameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 24));
        lightAttack.SetCombatController(COMBAT_TYPE.ATTACK_LIGHT, 1f);
        lightAttack.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 27));
        lightAttack.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, animationInfo.maxFrame));
        EndSkill();
    }
}
