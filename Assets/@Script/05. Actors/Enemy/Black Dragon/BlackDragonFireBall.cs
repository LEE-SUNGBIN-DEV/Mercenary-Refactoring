using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonFireBall : EnemySkill
{
    [SerializeField] private Transform muzzle;
    private AnimationClipInfo animationClipInformation;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Fire_Ball";
        cooldown = 5;
        minAttackDistance = 15f;
        maxAttackDistance = 30f;

        muzzle = Functions.FindChild<Transform>(gameObject, "Muzzle", true);
        enemy.ObjectPooler.RegistObject(Constants.VFX_Black_Dragon_Fire_Ball, 2);
        
        animationClipInformation = enemy.AnimationClipTable["Skill_Fire_Ball"];
    }

    public override IEnumerator CoStartSkill()
    {
        enemy.Animator.Play(animationClipInformation.nameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(animationClipInformation, 27));

        GameObject fireBall = enemy.ObjectPooler.RequestObject(Constants.VFX_Black_Dragon_Fire_Ball);
        fireBall.transform.position = muzzle.position;

        if (fireBall.TryGetComponent(out EnemyProjectile projectile))
        {
            projectile.SetCombatController(HIT_TYPE.HEAVY, GUARD_TYPE.NONE, 1.5f);
            projectile.SetProjectile(enemy, 20f, transform.forward);
        }

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
