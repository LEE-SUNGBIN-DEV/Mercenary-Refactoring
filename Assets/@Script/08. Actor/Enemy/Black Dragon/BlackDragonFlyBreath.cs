using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonFlyBreath : EnemySkill
{
    [SerializeField] private EnemyBreath breath;

    private AnimationInfo flyBreathStartAnimationInfo;
    private AnimationInfo flyBreathAnimationInfo;
    private AnimationInfo flyBreathEndAnimationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Fly_Breath";
        cooldown = 45f;
        minAttackDistance = 0f;
        maxAttackDistance = 15f;

        breath = Functions.FindChild<EnemyBreath>(gameObject, "Breath Controller", true);

        enemy.ObjectPooler.RegisterObject(Constants.VFX_Enemy_Breath, 15);
        enemy.ObjectPooler.RegisterObject(Constants.VFX_Enemy_Flame_Area, 15);

        flyBreathStartAnimationInfo = new AnimationInfo("Skill_Fly_Breath_Start", 2.125f, 51, 1.2f);
        flyBreathAnimationInfo = new AnimationInfo(skillName, 5.625f, 135, 1.2f);
        flyBreathEndAnimationInfo = new AnimationInfo("Skill_Fly_Breath_End", 4.167f, 100, 1.5f);
    }

    public override IEnumerator StartSkill()
    {
        enemy.Animator.Play(flyBreathStartAnimationInfo.animationNameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(flyBreathAnimationInfo, 55));
        breath.SetCombatController(COMBAT_TYPE.ATTACK_LIGHT, 1f);
        breath.SetRayAttack(enemy, 30f, 0.1f);
        StartCoroutine(breath.RayCoroutine);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(flyBreathAnimationInfo, 103));
        StopCoroutine(breath.RayCoroutine);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(flyBreathEndAnimationInfo, flyBreathEndAnimationInfo.maxFrame));
        EndSkill();
    }
}