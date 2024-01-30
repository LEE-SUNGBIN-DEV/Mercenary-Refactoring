using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoledGroundSlam : EnemySkill
{
    [SerializeField] private EnemyCompeteAttack groundSlamController;
    [SerializeField] private ParticleController groundSlamVFX;
    private AnimationClipInfo animationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Ground_Slam";
        cooldown = 12f;
        minAttackDistance = 0f;
        maxAttackDistance = 4f;

        animationInfo = enemy.AnimationClipTable["Skill_Ground_Slam"];

        groundSlamController = Functions.FindChild<EnemyCompeteAttack>(gameObject, "Ground_Slam_Controller", true);
        groundSlamController.SetCompeteAttack(enemy);

        // VFX
        groundSlamVFX = Functions.FindChild<ParticleController>(gameObject, "VFX_Ground_Slam", true);
        groundSlamVFX.Initialize(PARTICLE_MODE.AUTO_DISABLE, 5f);
        groundSlamVFX.gameObject.SetActive(false);
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.SFXPlayer.PlaySFX("Audio_Big_Golem_Attack_02");
        enemy.Animator.CrossFadeInFixedTime(animationInfo.nameHash, 0.2f);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 65));
        groundSlamController.SetCombatController(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 3f);
        groundSlamController.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 69));
        groundSlamController.OnDisableCollider();
        groundSlamVFX.gameObject.SetActive(true);
        enemy.SFXPlayer.PlaySFX(Constants.Audio_Ground_Slam);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, animationInfo.maxFrame));
        EndSkill();
    }

    public override IEnumerator CoLookTarget()
    {
        while (!enemy.Animator.IsAnimationFrameUpTo(animationInfo, 60))
        {
            enemy.LookTarget();
            yield return null;
        }
    }
}
