using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKnightVerticalSlash : EnemySkill
{
    [SerializeField] private Collider attackCollider;

    private void Awake()
    {
        isRotate = false;
        isReady = true;
    }

    public override void ActiveSkill()
    {
        StartCoroutine(WaitForRotate());
        Owner.Animator.SetTrigger("doAttack1");
        StartCoroutine(SkillCooldown());
    }

    #region Animation Event Function
    public void OnVerticalSlashCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }
    }
    public void OffVerticalSlashCollider()
    {
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }
    #endregion
}
