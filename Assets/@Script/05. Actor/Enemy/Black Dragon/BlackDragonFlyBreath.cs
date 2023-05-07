using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonFlyBreath : EnemySkill
{
    [SerializeField] private EnemyBreath breath;

    private AnimationClipInformation flyBreathStartAnimationInfo;
    private AnimationClipInformation flyBreathAnimationInfo;
    private AnimationClipInformation flyBreathEndAnimationInfo;

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
        
        flyBreathStartAnimationInfo = enemy.AnimationClipTable["Skill_Fly_Breath_Start"];
        flyBreathAnimationInfo = enemy.AnimationClipTable["Skill_Fly_Breath"];
        flyBreathEndAnimationInfo = enemy.AnimationClipTable["Skill_Fly_Breath_End"];
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.Play(flyBreathStartAnimationInfo.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(flyBreathAnimationInfo, 55));
        breath.SetCombatController(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1f);
        breath.SetRayAttack(enemy, 30f, 0.1f);
        StartCoroutine(breath.RayCoroutine);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(flyBreathAnimationInfo, 103));
        StopCoroutine(breath.RayCoroutine);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(flyBreathEndAnimationInfo, flyBreathEndAnimationInfo.maxFrame));
        EndSkill();
    }

    public override IEnumerator CoLookTarget()
    {
        while (!enemy.Animator.IsAnimationFrameUpTo(flyBreathStartAnimationInfo, 20))
        {
            enemy.LookTarget();
            yield return null;
        }
    }
}
