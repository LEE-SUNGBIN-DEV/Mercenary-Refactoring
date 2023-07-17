using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonStorm : EnemySkill
{
    [SerializeField] private int amount;
    [SerializeField] private float interval;
    private AnimationClipInformation stormStartAnimationInfo;
    private AnimationClipInformation stormAnimationInfo;
    private AnimationClipInformation stormEndAnimationInfo;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        skillName = "Skill_Storm";
        cooldown = 30f;
        minAttackDistance = 0f;
        maxAttackDistance = 15f;

        owner.ObjectPooler.RegistObject(Constants.VFX_Black_Dragon_Storm_Field, 1);
        owner.ObjectPooler.RegistObject(Constants.VFX_Black_Dragon_Lightning_Strike, amount);        
        
        stormStartAnimationInfo = enemy.AnimationClipTable["Skill_Storm_Start"];
        stormAnimationInfo = enemy.AnimationClipTable["Skill_Storm"];
        stormEndAnimationInfo = enemy.AnimationClipTable["Skill_Storm_End"];
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.Play(stormStartAnimationInfo.nameHash);

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
            Vector2 randomCoordinate = Random.insideUnitCircle * 12f;
            if (enemy.ObjectPooler.RequestObject(Constants.VFX_Black_Dragon_Lightning_Strike).TryGetComponent(out EnemyPositioningAttack lightningStrike))
            {
                lightningStrike.SetCombatController(HIT_TYPE.STUN, GUARD_TYPE.NONE, 1.3f, 1.5f);
                lightningStrike.EnablePositioningAttack(enemy, enemy.transform.position + new Vector3(randomCoordinate.x, 0, randomCoordinate.y), 2f, 1f, 0.2f);
            }

            yield return waitTime;
        }
    }

    public override IEnumerator CoLookTarget()
    {
        while (!enemy.Animator.IsAnimationFrameUpTo(stormStartAnimationInfo, stormStartAnimationInfo.maxFrame))
        {
            enemy.LookTarget();
            yield return null;
        }
    }
}
