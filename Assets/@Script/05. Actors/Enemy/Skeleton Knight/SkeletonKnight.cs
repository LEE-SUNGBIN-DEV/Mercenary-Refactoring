using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonKnight : BaseEnemy, ICompetable
{
    public override void InitializeEnemy(int enemyID)
    {
        base.InitializeEnemy(enemyID);
    }

    public override void Update()
    {
        base.Update();
    }

    #region Override Function
    public override void OnLightHit()
    {
    }

    public override void OnHeavyHit()
    {
    }

    public virtual void OnStun(float duration)
    {
        Animator.SetBool("isMove", false);
        Animator.SetBool("isStun", true);

        StartCoroutine(StunTime());
    }

    #endregion

    public IEnumerator StunTime(float time = 4f)
    {
        yield return new WaitForSeconds(time);

        Animator.SetBool("isStun", false);
    }

    public void OnCompete()
    {
        if (IsCompete || IsDie)
            return;

        // Initialize Previous State

        // Compete State
        IsCompete = true;
        Animator.SetTrigger("doCompete");

        StartCoroutine(CompeteTime());
    }
    public IEnumerator CompeteTime()
    {
        yield return new WaitForSeconds(Constants.TIME_COMPETE);
        Animator.SetTrigger("doCompeteAttack");
        status.CurrentHP -= (status.MaxHP * 0.1f);
        IsCompete = false;
        Stagger();
    }
    public void Stagger()
    {
        Animator.SetBool("isMove", false);
        Animator.SetBool("isStun", true);
    }
    #region Animation Event Function
    public void OutCompete()
    {
        IsCompete = false;
    }
    #endregion

    #region Property
    public bool IsCompete { get; set; }
    #endregion
}
