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
    protected IEnumerator lookTargetCoroutine;

    public virtual void Initialize()
    {
        enemy = GetComponentInParent<BaseEnemy>(true);
        isReady = true;
        RegisterCoroutine();
    }

    public virtual void Initialize(BaseEnemy enemy)
    {
        this.enemy = enemy;
        isReady = true;
        RegisterCoroutine();
    }

    public void OnDisable()
    {
        DisableSkill();
    }

    public virtual void EnableSkill()
    {
        RegisterCoroutine();
        StartCoroutine(cooldownCoroutine);
        StartCoroutine(skillCoroutine);
        StartCoroutine(lookTargetCoroutine);
    }

    public abstract IEnumerator CoStartSkill();
    public abstract IEnumerator CoLookTarget();

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

    public void RegisterCoroutine()
    {
        cooldownCoroutine = WaitForCooldown();
        skillCoroutine = CoStartSkill();
        lookTargetCoroutine = CoLookTarget();
    }
    public virtual void DisableSkill()
    {
        if (skillCoroutine != null)
        {
            StopCoroutine(skillCoroutine);
            StopCoroutine(lookTargetCoroutine);
        }
    }

    #region Property
    public BaseEnemy Enemy { get { return enemy; } }
    #endregion
}
