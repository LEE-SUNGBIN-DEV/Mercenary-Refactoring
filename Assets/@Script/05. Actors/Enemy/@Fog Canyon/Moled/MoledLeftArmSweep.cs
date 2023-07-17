using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoledLeftArmSweep : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack leftArmSweepController;
    [SerializeField] private ParticleController leftArmSweepSmokeVFX;
    private AnimationClipInformation animationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Left_Arm_Sweep";
        cooldown = 10f;
        minAttackDistance = 0f;
        maxAttackDistance = 3f;

        animationInfo = enemy.AnimationClipTable["Skill_Left_Arm_Sweep"];

        leftArmSweepController = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Left_Arm_Sweep_Controller", true);
        leftArmSweepController.SetMeleeAttack(enemy);

        // VFX
        leftArmSweepSmokeVFX = Functions.FindChild<ParticleController>(gameObject, "VFX_Left_Arm_Sweep_Smoke", true);
        leftArmSweepSmokeVFX.Initialize(PARTICLE_MODE.AUTO_DISABLE, 5f);
        leftArmSweepSmokeVFX.gameObject.SetActive(false);
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.CrossFadeInFixedTime(animationInfo.nameHash, 0.2f);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 72));
        leftArmSweepSmokeVFX.gameObject.SetActive(true);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 78));
        leftArmSweepController.SetCombatController(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2f);
        leftArmSweepController.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 107));
        leftArmSweepController.OnDisableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, animationInfo.maxFrame));
        EndSkill();
    }

    public override IEnumerator CoLookTarget()
    {
        while (!enemy.Animator.IsAnimationFrameUpTo(animationInfo, 40))
        {
            enemy.LookTarget();
            yield return null;
        }
    }
}
