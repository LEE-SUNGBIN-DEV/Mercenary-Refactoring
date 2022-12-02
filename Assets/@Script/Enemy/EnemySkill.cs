using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySkill : MonoBehaviour
{
    [SerializeField] protected Enemy owner;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float maxRange;
    protected bool isRotate;
    protected bool isReady;

    public virtual void Update()
    {
    }
    public abstract void ActiveSkill();

    public virtual bool CheckCondition(float targetDistance)
    {
        return (isReady && (targetDistance <= MaxRange));
    }
    public IEnumerator WaitForRotate()
    {
        isRotate = true;
        yield return new WaitForSeconds(0.5f);
        isRotate = false;
    }
    public IEnumerator SkillCooldown()
    {
        isReady = false;
        yield return new WaitForSeconds(Cooldown);
        isReady = true;
    }

    #region Property
    public Enemy Owner
    {
        get { return owner; }
        set { owner = value; }
    }
    public float MaxRange
    {
        get { return maxRange; }
    }
    public float Cooldown
    {
        get { return cooldown; }
    }
    public bool IsRotate
    {
        get { return isRotate; }
    }
    public bool IsReady
    {
        get { return isReady; }
    }
    #endregion
}
