using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoledLeftArmAttack : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack leftArmController;
    [SerializeField] private ParticleController leftArmAttackVFX;
    private AnimationClipInformation animationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Left_Arm_Attack";
        cooldown = 10f;
        minAttackDistance = 0f;
        maxAttackDistance = 4f;

        animationInfo = enemy.AnimationClipTable["Skill_Left_Arm_Attack"];

        leftArmController = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Left_Arm_Controller", true);
        leftArmController.SetMeleeAttack(enemy);

        // VFX
        leftArmAttackVFX = Functions.FindChild<ParticleController>(gameObject, "VFX_Left_Arm_Attack", true);
        leftArmAttackVFX.Initialize(PARTICLE_MODE.AUTO_DISABLE, 5f);
        leftArmAttackVFX.gameObject.SetActive(false);
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.CrossFadeInFixedTime(animationInfo.nameHash, 0.2f);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 58));
        leftArmController.SetCombatController(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 2f);
        leftArmController.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 65));
        leftArmController.OnDisableCollider();
        leftArmAttackVFX.gameObject.SetActive(true);
        enemy.SFXPlayer.PlaySFX(Constants.Audio_Ground_Slam);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, animationInfo.maxFrame));
        EndSkill();
    }

    public override IEnumerator CoLookTarget()
    {
        while (!enemy.Animator.IsAnimationFrameUpTo(animationInfo, 58))
        {
            enemy.LookTarget();
            yield return null;
        }
    }
}
