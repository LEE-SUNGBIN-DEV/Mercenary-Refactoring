using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonFireBall : EnemySkill
{
    [SerializeField] private float minRange;
    [SerializeField] private GameObject muzzle;

    public override bool CheckCondition(float targetDistance)
    {
        return (isReady && (targetDistance <= maxRange) && (targetDistance >= minRange));
    }
    public override void ActiveSkill()
    {
        base.ActiveSkill();
        Owner.Animator.SetTrigger("doFireBall");
    }

    #region Animation Event Function
    public void OnFireBall()
    {
        GameObject fireBall = owner.RequestObject("Prefab_Black_Dragon_Fire_Ball");
        fireBall.transform.position = muzzle.transform.position;

        if(fireBall.TryGetComponent(out EnemyProjectile projectile))
        {
            projectile.Owner = owner;
            projectile.transform.forward = transform.forward;
        }
    }
    #endregion
}
