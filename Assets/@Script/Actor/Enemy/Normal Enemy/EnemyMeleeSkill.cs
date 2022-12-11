using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeSkill : EnemySkill
{
    [SerializeField] private Collider attackCollider;

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
    public void OnAttackCollider()
    {
        if(attackCollider != null)
        {
            attackCollider.enabled = true;
        }
    }
    public void OffAttackCollider()
    {
        if(attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }
    #endregion
}
