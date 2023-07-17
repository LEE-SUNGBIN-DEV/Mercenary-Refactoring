using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoledGroundBurst : EnemySkill
{
    private AnimationClipInformation animationInfo;
    private float chaseDuration = 4f;
    private float generateInterval = 1f;
    private float lifeTime = 9.5f;
    private float delayTime = 2f;
    private float attackDuration = 1f;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Ground_Burst";
        cooldown = 15f;
        minAttackDistance = 0f;
        maxAttackDistance = 10f;

        animationInfo = enemy.AnimationClipTable["Skill_Ground_Burst"];

        enemy.ObjectPooler.RegistObject(Constants.VFX_Ground_Burst, 5);
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.CrossFadeInFixedTime(animationInfo.nameHash, 0.2f);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationInfo, 125));
        StartCoroutine(GroundBurst());

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

    public IEnumerator GroundBurst()
    {
        WaitForSeconds interval = new WaitForSeconds(generateInterval);
        float currentDuration = 0f;

        while (chaseDuration > currentDuration)
        {
            currentDuration += generateInterval;

            Vector3 spawnPosition = enemy.TargetTransform.position;
            spawnPosition.y += 0.15f;

            Vector3 randomRotation = new Vector3(0, Random.Range(0, 360), 0);
            GenerateGroundBurst(spawnPosition, Quaternion.Euler(randomRotation));

            yield return interval;
        }
    }

    public void GenerateGroundBurst(Vector3 spawnPosition, Quaternion rotation)
    {
        if (enemy.ObjectPooler.RequestObject(Constants.VFX_Ground_Burst).TryGetComponent(out EnemyPositioningAttack groundBurst))
        {
            groundBurst.transform.localScale = Vector3.one * 3f;
            groundBurst.SetCombatController(HIT_TYPE.LIGHT, GUARD_TYPE.NONE, 1f);
            groundBurst.EnablePositioningAttack(enemy, spawnPosition, rotation, lifeTime, delayTime, attackDuration);
            groundBurst.StartCoroutine(groundBurst.CoPlaySFX(Constants.Audio_Ground_Burst, delayTime));
        }
    }
}
