using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPositioningAttack : EnemyCombatController, IPoolObject
{
    [Header("Positioning Attack")]
    [SerializeField] private float delayTime;
    [SerializeField] private float attackTime;
    [SerializeField] private float duration;
    [SerializeField] private Vector3 targetPosition;
    private IEnumerator delayAttackCoroutine;
    private IEnumerator autoReturnCoroutine;
    private ObjectPooler objectPooler;

    public void SetPositioningAttack(BaseEnemy owner, Vector3 targetPosition, float delayTime, float attackTime)
    {
        this.owner = owner;
        this.delayTime = delayTime;
        this.attackTime = attackTime;
        this.targetPosition = targetPosition;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other != null)
            ExecuteAttackProcess(other);
    }

    public void OnAttack()
    {
        transform.position = targetPosition;
        if (delayAttackCoroutine != null)
            StartCoroutine(delayAttackCoroutine);

        if (autoReturnCoroutine != null)
            StartCoroutine(autoReturnCoroutine);
    }

    public IEnumerator CoDelayAttack()
    {
        yield return new WaitForSeconds(delayTime);
        combatCollider.enabled = true;
        yield return new WaitForSeconds(attackTime);
        combatCollider.enabled = false;
    }
    public IEnumerator CoAutoReturn()
    {
        yield return new WaitForSeconds(duration);
        ReturnOrDestoryObject(objectPooler);
    }

    #region IPoolObject Interface Fucntion
    public void ActionAfterRequest(ObjectPooler owner)
    {
        objectPooler = owner;
        delayAttackCoroutine = CoDelayAttack();
        autoReturnCoroutine = CoAutoReturn();

        if (combatCollider != null)
            combatCollider.enabled = false;
    }
    public void ActionBeforeReturn()
    {
        if (combatCollider != null)
            combatCollider.enabled = false;

        if (delayAttackCoroutine != null)
            StopCoroutine(delayAttackCoroutine);

        if (autoReturnCoroutine != null)
            StopCoroutine(autoReturnCoroutine);
    }
    public void ReturnOrDestoryObject(ObjectPooler owner)
    {
        if (owner == null)
            Destroy(gameObject);

        owner.ReturnObject(name, gameObject);
    }
    public ObjectPooler ObjectPooler { get { return objectPooler; } }
    #endregion
}
