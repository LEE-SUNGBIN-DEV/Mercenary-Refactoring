using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeSkill : EnemySkill
{
    [SerializeField] private GameObject muzzle;
    [SerializeField] private string key;

    private void Awake()
    {
        isReady = true;
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        Owner.Animator.SetTrigger("doAttack");
    }

    #region Animation Event Function
    public void OnProjectile()
    {
        GameObject projectile = Managers.ObjectPoolManager.RequestObject(key);
        projectile.transform.position = muzzle.transform.position;

        EnemyProjectile monsterProjectile = projectile.GetComponent<EnemyProjectile>();
        monsterProjectile.Owner = GetComponent<Enemy>();
        monsterProjectile.transform.forward = transform.forward;
    }
    #endregion
}
