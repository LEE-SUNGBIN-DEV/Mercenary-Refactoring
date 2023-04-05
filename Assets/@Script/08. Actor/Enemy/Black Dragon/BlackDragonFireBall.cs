using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonFireBall : EnemySkill
{
    [SerializeField] private Transform muzzle;
    private AnimationInfo fireBallAnimationInfo;

    public override void Initialize(BaseEnemy enemy)
    {
        base.Initialize(enemy);
        skillName = "Skill_Fire_Ball";
        cooldown = 5;
        minAttackDistance = 15f;
        maxAttackDistance = 30f;

        muzzle = Functions.FindChild<Transform>(gameObject, "Muzzle", true);
        enemy.ObjectPooler.RegisterObject(Constants.VFX_Black_Dragon_Fire_Ball, 2);

        fireBallAnimationInfo = new AnimationInfo(skillName, 2.625f, 63, 1f);
    }

    public override IEnumerator StartSkill()
    {
        enemy.Animator.Play(fireBallAnimationInfo.animationNameHash);

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(fireBallAnimationInfo, 27));

        GameObject fireBall = enemy.ObjectPooler.RequestObject(Constants.VFX_Black_Dragon_Fire_Ball);
        fireBall.transform.position = muzzle.position;

        if (fireBall.TryGetComponent(out EnemyProjectile projectile))
        {
            projectile.SetCombatController(COMBAT_TYPE.ATTACK_HEAVY, 1.5f);
            projectile.SetProjectile(enemy, 20f, transform.forward);
        }

        yield return new WaitUntil(() => enemy.Animator.IsAnimationFrameUpTo(fireBallAnimationInfo, fireBallAnimationInfo.maxFrame));
        EndSkill();
    }
}
