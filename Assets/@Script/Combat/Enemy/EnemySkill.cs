using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySkill : MonoBehaviour
{
    [Header("Enemy Skill")]
    [SerializeField] protected Enemy owner;
    [SerializeField] protected Collider skillCollider;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float maxRange;
    protected bool isReady;

    // Initialize
    public virtual void Initialize()
    {
        this.owner = GetComponentInParent<Enemy>(true);
        this.skillCollider = GetComponentInChildren<Collider>(true);
        isReady = true;
    }
    public virtual void Initialize(Enemy owner)
    {
        this.owner = owner;
        this.skillCollider = GetComponentInChildren<Collider>(true);
        isReady = true;
    }

    // 
    public virtual void ActiveSkill()
    {
        StartCoroutine(WaitForCooldown());
    }
    public virtual bool CheckCondition(float targetDistance)
    {
        return (isReady && (targetDistance <= maxRange));
    }
    public IEnumerator WaitForCooldown()
    {
        isReady = false;
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }

    #region Animation Event Function
    public void EnableSkillCollider(bool isEnable)
    {
        if (skillCollider != null)
            skillCollider.enabled = isEnable;
    }
    #endregion

    #region Property
    public Enemy Owner { get { return owner; } set { owner = value; } }
    public Collider SkillCollider { get { return skillCollider; } }
    #endregion
}
