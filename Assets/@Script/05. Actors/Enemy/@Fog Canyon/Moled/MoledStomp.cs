using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoledStomp : EnemySkill
{
    [SerializeField] private EnemyMeleeAttack stompController;
    [SerializeField] private ParticleController stompVFX;
    private AnimationClipInformation animationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Stomp";
        cooldown = 10f;
        minAttackDistance = 0f;
        maxAttackDistance = 2;

        animationInfo = enemy.AnimationClipTable["Skill_Stomp"];

        stompController = Functions.FindChild<EnemyMeleeAttack>(gameObject, "Stomp_Controller", true);
        stompController.SetMeleeAttack(enemy);

        // VFX
        stompVFX = Functions.FindChild<ParticleController>(gameObject, "VFX_Stomp", true);
        stompVFX.Initialize(PARTICLE_MODE.AUTO_DISABLE, 5f);
        stompVFX.gameObject.SetActive(false);
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.CrossFadeInFixedTime(animationInfo.nameHash, 0.2f);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 46));
        stompController.SetCombatController(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1.5f);
        stompController.OnEnableCollider();

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 52));
        stompController.OnDisableCollider();
        stompVFX.gameObject.SetActive(true);
        enemy.SFXPlayer.PlaySFX(Constants.Audio_Ground_Slam);

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
