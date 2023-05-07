using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonLandBreath : EnemySkill
{
    [SerializeField] private EnemyBreath breath;
    private AnimationClipInformation animationClipInformation;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Land_Breath";
        cooldown = 30f;
        minAttackDistance = 0f;
        maxAttackDistance = 15f;

        breath = Functions.FindChild<EnemyBreath>(gameObject, "Breath Controller", true);

        enemy.ObjectPooler.RegisterObject(Constants.VFX_Enemy_Breath, 15);
        enemy.ObjectPooler.RegisterObject(Constants.VFX_Enemy_Flame_Area, 15);

        animationClipInformation = enemy.AnimationClipTable["Skill_Land_Breath"];
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.Play(animationClipInformation.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, 23));
        breath.SetCombatController(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1f);
        breath.SetRayAttack(enemy, 20f, 0.15f);
        StartCoroutine(breath.RayCoroutine);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, 72));
        StopCoroutine(breath.RayCoroutine);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, animationClipInformation.maxFrame));
        EndSkill();
    }

    public override IEnumerator CoLookTarget()
    {
        while (!enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, 20))
        {
            enemy.LookTarget();
            yield return null;
        }
    }
}
