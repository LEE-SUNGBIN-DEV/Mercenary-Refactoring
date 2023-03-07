using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonStorm : EnemySkill
{
    [SerializeField] private int amount;
    [SerializeField] private float interval;
    private AnimationInfo stormStartAnimationInfo;
    private AnimationInfo stormAnimationInfo;
    private AnimationInfo stormEndAnimationInfo;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        skillName = "Skill_Storm";
        cooldown = 30f;
        minAttackDistance = 0f;
        maxAttackDistance = 15f;

        owner.ObjectPooler.RegisterObject(Constants.VFX_Black_Dragon_Storm_Field, 1);
        owner.ObjectPooler.RegisterObject(Constants.VFX_Black_Dragon_Lightning_Strike, amount);

        stormStartAnimationInfo = new AnimationInfo("Skill_Storm_Start", 2.125f, 51, 1f);
        stormAnimationInfo = new AnimationInfo(skillName, 5.000f, 120, 1f);
        stormEndAnimationInfo = new AnimationInfo("Skill_Storm_End", 4.167f, 100, 1.5f);
    }

    public override IEnumerator StartSkill()
    {
        enemy.Animator.Play(stormStartAnimationInfo.animationNameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(stormStartAnimationInfo, 0));
        GameObject stormField = enemy.ObjectPooler.RequestObject(Constants.VFX_Black_Dragon_Storm_Field);
        if (stormField != null)
            stormField.transform.position = enemy.transform.position;

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(stormAnimationInfo, 0));
        StartCoroutine(GenerateLightningStrike());

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(stormEndAnimationInfo, stormEndAnimationInfo.maxFrame));
        EndSkill();
    }

    public IEnumerator GenerateLightningStrike()
    {
        WaitForSeconds waitTime = new WaitForSeconds(interval);

        for (int i = 0; i < amount; ++i)
        {
            Vector3 generateCoordinate = Functions.GetRandomCircleCoordinate(12f);
            if (enemy.ObjectPooler.RequestObject(Constants.VFX_Black_Dragon_Lightning_Strike).TryGetComponent(out EnemyPositioningAttack lightningStrike))
            {
                lightningStrike.SetCombatController(COMBAT_TYPE.ATTACK_STUN, 1.3f, 1.5f);
                lightningStrike.SetPositioningAttack(enemy, enemy.transform.position + generateCoordinate, 1f, 0.2f);
                lightningStrike.OnAttack();
            }

            yield return waitTime;
        }
    }
}
