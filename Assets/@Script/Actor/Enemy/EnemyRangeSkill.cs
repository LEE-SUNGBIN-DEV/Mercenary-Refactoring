using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeSkill : EnemySkill
{
    [SerializeField] private GameObject muzzle;
    [SerializeField] private string key;

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        Owner.Animator.SetTrigger("doAttack");
    }

    #region Animation Event Function
    private void OnRangeAttack()
    {
        if(owner.ObjectPooler.RequestObject(key).TryGetComponent(out EnemyProjectile enemyProjectile))
        {
            enemyProjectile.transform.position = muzzle.transform.position;
            enemyProjectile.SetProjectile(owner, 5f, transform.forward);
        }
    }
    #endregion
}
