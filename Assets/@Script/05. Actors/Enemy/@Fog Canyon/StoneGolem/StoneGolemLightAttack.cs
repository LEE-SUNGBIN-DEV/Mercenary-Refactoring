using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGolemLightAttack : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack lightAttack;
    private AnimationClipInformation animationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Light_Attack";
        cooldown = 5f;
        minAttackDistance = 0f;
        maxAttackDistance = 1f;

        lightAttack = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Light_Attack_Controller", true);
        lightAttack.SetMeleeAttack(enemy);

        animationInfo = enemy.AnimationClipTable["Skill_Light_Attack"];
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.Play(animationInfo.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 24));
        lightAttack.SetCombatController(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1f);
        lightAttack.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 27));
        lightAttack.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, animationInfo.maxFrame));
        EndSkill();
    }

    public override IEnumerator CoLookTarget()
    {
        while (!enemy.Animator.IsAnimationFrameUpTo(animationInfo, 16))
        {
            enemy.LookTarget();
            yield return null;
        }
    }
}
