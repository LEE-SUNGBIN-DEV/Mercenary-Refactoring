using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKnightHorizontalSlash : EnemySkill
{
    [SerializeField] private Collider attackCollider;

    private void Awake()
    {
        IsRotate = false;
        IsReady = true;
    }

    public override void ActiveSkill()
    {
        Owner.StopTrace();
        StartCoroutine(WaitForRotate());
        Owner.Animator.SetTrigger("doAttack2");
        StartCoroutine(SkillCooldown());
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
