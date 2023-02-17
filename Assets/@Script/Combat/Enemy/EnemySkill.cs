using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemySkill : MonoBehaviour
{
    public event UnityAction<bool> OnSKillEnd;

    [Header("Enemy Skill")]
    [SerializeField] protected BaseEnemy owner;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float maxRange;
    protected bool isReady;

    // Initialize
    public virtual void Initialize()
    {
        owner = GetComponentInParent<BaseEnemy>(true);
        isReady = true;
    }
    public virtual void Initialize(BaseEnemy owner)
    {
        this.owner = owner;
        isReady = true;
    }

    public virtual void ActiveSkill()
    {
        StartCoroutine(WaitForCooldown());
    }
    public virtual bool IsReady(float targetDistance)
    {
        return (isReady && (targetDistance <= maxRange));
    }
    public IEnumerator WaitForCooldown()
    {
        isReady = false;
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }

    #region Property
    public BaseEnemy Owner { get { return owner; } set { owner = value; } }
    #endregion
}
