using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyCombatController, IPoolObject
{
    [Header("Enemy Projectile")]
    [SerializeField] private float speed;
    [SerializeField] private float duration;
    private IEnumerator autoReturnCoroutine;
    private ObjectPooler objectPooler;

    public void SetProjectile(BaseEnemy enemy, Vector3 direction)
    {
        this.enemy = enemy;
        transform.forward = direction;
        autoReturnCoroutine = CoAutoReturn();
    }
    public void SetProjectile(BaseEnemy enemy, float speed, Vector3 direction)
    {
        this.enemy = enemy;
        this.speed = speed;
        transform.forward = direction;
        autoReturnCoroutine = CoAutoReturn();
    }

    private void Update()
    {
        if (enemy == null) Destroy(gameObject);
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other != null)
            ExecuteAttackProcess(other);

        if (other.gameObject.layer == (int)PHYSICS_LAYER.Terrain)
        {
            OnHitWithTerrain(other);
            ReturnOrDestoryObject(objectPooler);
        }
    }

    public virtual void OnHitWithTerrain(Collider other)
    {
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
        OnEnableCollider();

        if (autoReturnCoroutine != null)
            StartCoroutine(autoReturnCoroutine);
    }

    public void ActionBeforeReturn()
    {
        OnDisableCollider();

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
