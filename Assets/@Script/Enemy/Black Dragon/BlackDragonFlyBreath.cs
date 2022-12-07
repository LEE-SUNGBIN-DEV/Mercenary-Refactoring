using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackDragonFlyBreath : EnemySkill
{
    [SerializeField] private GameObject attackEffect;

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        Owner.Animator.SetTrigger("doFlyBreath");
    }

    #region Animation Event Function
    public void OnFlyBreath()
    {
        if (attackEffect != null)
            attackEffect.SetActive(true);
    }

    public void OffFlyBreath()
    {
        if (attackEffect != null)
            attackEffect.SetActive(false);
    }
    #endregion
}
