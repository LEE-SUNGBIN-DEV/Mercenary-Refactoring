using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKnightHorizontalSlash : EnemySkill
{
    [SerializeField] private Collider attackCollider;

    private void Awake()
    {
        isReady = true;
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        Owner.Animator.SetTrigger("doAttack2");
    }

    #region Animation Event Function
    public void OnHorizontalSlashCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }
    }
    public void OffHorizontalSlashCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }
    #endregion
}
