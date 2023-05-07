using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonKnight : BaseEnemy, ICompetable
{
    protected override void Awake()
    {
        base.Awake();
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

        yield return new WaitForSeconds(Constants.TIME_COMPETE_ATTACK);
        status.CurrentHP -= (status.MaxHP * 0.1f);
        IsCompete = false;
        Stagger();
    }
    public void Stagger()
    {
        Animator.SetBool("isMove", false);
        Animator.SetBool("isStun", true);

        StartCoroutine(StunTime(Constants.TIME_STAGGER));
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
