using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGolemSlamAttack : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack slamAttack;
    private AnimationClipInformation animationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Slam_Attack";
        cooldown = 6f;
        minAttackDistance = 0f;
        maxAttackDistance = 4.5f;

        enemy.ObjectPooler.RegistObject("VFX_Ground_Slam", 2);

        slamAttack = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Slam_Attack_Controller", true);
        slamAttack.SetMeleeAttack(enemy);

        animationInfo = enemy.AnimationClipTable["Skill_Slam_Attack"];
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.CrossFadeInFixedTime(animationInfo.nameHash, 0.2f);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 54));
        slamAttack.SetCombatController(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2f);
        slamAttack.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 59));
        slamAttack.OnDisableCollider();
        enemy.SFXPlayer.PlaySFX("Audio_Ground_Slam");
        GameObject vfxObject = enemy.ObjectPooler.RequestObject("VFX_Ground_Slam");
        vfxObject.transform.position = slamAttack.transform.position;

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, animationInfo.maxFrame));
        EndSkill();
    }

    public override IEnumerator CoLookTarget()
    {
        while (!enemy.Animator.IsAnimationFrameUpTo(animationInfo, 48))
        {
            enemy.LookTarget();
            yield return null;
        }
    }
}
