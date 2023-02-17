using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonFireBall : EnemySkill
{
    [SerializeField] private float minRange;
    [SerializeField] private Transform muzzle;

    public override void Initialize(BaseEnemy owner)
    {
        base.Initialize(owner);
        cooldown = 5;
        minRange = 15f;
        maxRange = 30f;

        muzzle = Functions.FindChild<Transform>(gameObject, "Muzzle", true);
        owner.ObjectPooler.RegisterObject(Constants.VFX_Black_Dragon_Fire_Ball, 2);
    }

    public override bool IsReady(float targetDistance)
    {
        return (isReady && (targetDistance <= maxRange) && (targetDistance >= minRange));
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        Owner.Animator.SetTrigger("doFireBall");
    }

    #region Animation Event Function
    private void OnFireBall()
    {
        GameObject fireBall = owner.ObjectPooler.RequestObject(Constants.VFX_Black_Dragon_Fire_Ball);
        fireBall.transform.position = muzzle.transform.position;

        if (fireBall.TryGetComponent(out EnemyProjectile projectile))
        {
            projectile.SetCombatController(COMBAT_TYPE.Heavy_Attack, 1.5f);
            projectile.SetProjectile(owner, 20f, transform.forward);
        }
    }
    #endregion
}
