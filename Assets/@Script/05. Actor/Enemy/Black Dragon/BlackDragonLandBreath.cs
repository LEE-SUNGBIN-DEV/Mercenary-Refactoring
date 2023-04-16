using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonLandBreath : EnemySkill
{
    [SerializeField] private EnemyBreath breath;
    private AnimationClipInformation landBreathAnimationInfo;

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

        landBreathAnimationInfo = enemy.AnimationClipDictionary["Skill_Land_Breath"];
    }

    public override IEnumerator StartSkill()
    {
        enemy.Animator.Play(landBreathAnimationInfo.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(landBreathAnimationInfo, 23));
        breath.SetCombatController(COMBAT_TYPE.ATTACK_LIGHT, 1f);
        breath.SetRayAttack(enemy, 20f, 0.15f);
        StartCoroutine(breath.RayCoroutine);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(landBreathAnimationInfo, 72));
        StopCoroutine(breath.RayCoroutine);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(landBreathAnimationInfo, landBreathAnimationInfo.maxFrame));
        EndSkill();
    }
}
