using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPositioningAttack : EnemyCombatController, IPoolObject
{
    [Header("Positioning Attack")]
    private Coroutine autoReturnCoroutine;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other != null)
            ExecuteAttackProcess(other);
    }

    public void EnablePositioningAttack(BaseEnemy owner, Vector3 spawnPosition, float lifeTime, float delayTime, float attackDuration)
    {
        enemy = owner;
        transform.position = spawnPosition;

        OnDelayAttack(delayTime, attackDuration);
        OnAutoReturn(lifeTime);
    }

    public void EnablePositioningAttack(BaseEnemy owner, Vector3 spawnPosition, Quaternion rotation, float lifeTime, float delayTime, float attackDuration)
    {
        EnablePositioningAttack(owner, spawnPosition, lifeTime, delayTime, attackDuration);
        transform.rotation = rotation;
    }

    // Auto Return
    public void OnAutoReturn(float lifeTime)
    {
        if (autoReturnCoroutine != null)
            StopCoroutine(autoReturnCoroutine);

        autoReturnCoroutine = StartCoroutine(CoAutoReturn(lifeTime));
    }

    public IEnumerator CoAutoReturn(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        ReturnOrDestoryObject();
    }

    #region Interface Fucntions : IPoolObject
    public void ActionAfterRequest(ObjectPooler owner)
    {
        ObjectPooler = owner;
        OnDisableCollider();
    }
    public void ActionBeforeReturn()
    {
        OnDisableCollider();

        if (delayAttackCoroutine != null)
            StopCoroutine(delayAttackCoroutine);

        if (autoReturnCoroutine != null)
            StopCoroutine(autoReturnCoroutine);
    }
    public void ReturnOrDestoryObject()
    {
        if (ObjectPooler == null)
            Destroy(gameObject);

        ObjectPooler.ReturnObject(name, gameObject);
    }

    public ObjectPooler ObjectPooler { get; set; }
    #endregion
}
