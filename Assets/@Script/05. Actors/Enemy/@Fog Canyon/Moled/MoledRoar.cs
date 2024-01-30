using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoledRoar : EnemySkill
{
    [SerializeField] private ParticleController roarVFX;
    private AnimationClipInfo animationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Roar";
        cooldown = float.PositiveInfinity;
        minAttackDistance = 0f;
        maxAttackDistance = float.PositiveInfinity;

        animationInfo = enemy.AnimationClipTable["Skill_Roar"];
        priority = 50;

        // VFX
        roarVFX = Functions.FindChild<ParticleController>(gameObject, "VFX_Roar", true);
        roarVFX.Initialize(PARTICLE_MODE.AUTO_DISABLE, 5f);
        roarVFX.gameObject.SetActive(false);
    }

    public override bool IsReady(float targetDistance)
    {
        return base.IsReady(targetDistance) && (enemy.Status.GetHPRatio() < 0.5f);
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.CrossFadeInFixedTime(animationInfo.nameHash, 0.2f);
        // Buffs
        //
        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 30));
        enemy.SFXPlayer.PlaySFX(Constants.Audio_Big_Golem_Roar);
        roarVFX.gameObject.SetActive(true);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 107));

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, animationInfo.maxFrame));
        EndSkill();
    }

    public override IEnumerator CoLookTarget()
    {
        while (!enemy.Animator.IsAnimationFrameUpTo(animationInfo, 30))
        {
            enemy.LookTarget();
            yield return null;
        }
    }
}
