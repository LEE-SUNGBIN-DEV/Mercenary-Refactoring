using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemySkill : MonoBehaviour
{
    public event UnityAction<bool> OnEndSkill;

    [Header("Enemy Skill")]
    [SerializeField] protected BaseEnemy enemy;
    [SerializeField] protected string skillName;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float minAttackDistance;
    [SerializeField] protected float maxAttackDistance;
    [SerializeField] protected bool isReady;
    protected IEnumerator cooldownCoroutine;
    protected IEnumerator skillCoroutine;

    public virtual void Initialize()
    {
        enemy = GetComponentInParent<BaseEnemy>(true);
        isReady = true;
        cooldownCoroutine = WaitForCooldown();
        skillCoroutine = StartSkill();
    }

    public virtual void Initialize(BaseEnemy enemy)
    {
        this.enemy = enemy;
        isReady = true;
        cooldownCoroutine = WaitForCooldown();
        skillCoroutine = StartSkill();
    }

    public void OnDisable()
    {
        if(skillCoroutine != null)
            StopCoroutine(skillCoroutine);
    }

    public virtual void ActiveSkill()
    {
        StartCoroutine(cooldownCoroutine);
        StartCoroutine(skillCoroutine);
    }

    public abstract IEnumerator StartSkill();

    public virtual bool IsReady(float targetDistance)
    {
        return (isReady && (targetDistance >= minAttackDistance) && (targetDistance <= maxAttackDistance));
    }

    public IEnumerator WaitForCooldown()
    {
        isReady = false;
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }

    public virtual void EndSkill()
    {
        OnEndSkill?.Invoke(true);
    }

    #region Property
    public BaseEnemy Enemy { get { return enemy; } }
    #endregion
}
