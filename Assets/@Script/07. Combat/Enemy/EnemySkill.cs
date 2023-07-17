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
    [SerializeField] protected int priority;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float minAttackDistance;
    [SerializeField] protected float maxAttackDistance;
    [SerializeField] protected bool isReady;
    protected Coroutine cooldownCoroutine;
    protected Coroutine skillCoroutine;
    protected Coroutine lookTargetCoroutine;

    public virtual void Initialize(BaseEnemy enemy)
    {
        this.enemy = enemy;
        isReady = true;
        priority = 0;
    }

    public void OnDisable()
    {
        //DisableSkill();
    }

    public virtual void EnableSkill()
    {
        if(cooldownCoroutine != null)
            StopCoroutine(cooldownCoroutine);
        cooldownCoroutine = StartCoroutine(CoStartCooldown());

        if (lookTargetCoroutine != null)
            StopCoroutine(lookTargetCoroutine);
        lookTargetCoroutine = StartCoroutine(CoLookTarget());

        if (skillCoroutine != null)
            StopCoroutine(skillCoroutine);
        skillCoroutine = StartCoroutine(CoStartSkill());
    }

    public abstract IEnumerator CoStartSkill();
    public abstract IEnumerator CoLookTarget();

    public virtual bool IsReady(float targetDistance)
    {
        return (isReady && (targetDistance >= minAttackDistance) && (targetDistance <= maxAttackDistance));
    }

    public IEnumerator CoStartCooldown()
    {
        isReady = false;
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }

    public virtual void EndSkill()
    {
        OnEndSkill?.Invoke(true);
    }

    public virtual void DisableSkill()
    {
        if (skillCoroutine != null)
            StopCoroutine(skillCoroutine);

        if (lookTargetCoroutine != null)
            StopCoroutine(lookTargetCoroutine);
    }

    #region Property
    public BaseEnemy Enemy { get { return enemy; } }
    public string SkillName { get { return skillName; } }
    public int Priority { get { return priority; } }
    public float Cooldown { get { return cooldown; } }
    public float MinAttackDistance { get { return minAttackDistance; } }
    public float MaxAttackDistance { get { return maxAttackDistance; } }
    #endregion
}
