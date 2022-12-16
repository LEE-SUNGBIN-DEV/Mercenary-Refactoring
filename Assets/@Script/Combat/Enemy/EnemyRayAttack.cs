using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRayAttack : EnemyCombatController
{
    [Header("Enemy Ray Attack")]
    [SerializeField] protected float rayDistance;
    [SerializeField] protected float rayInterval;
    protected IEnumerator rayCoroutine;

    public void SetRayAttack(BaseEnemy owner, float rayDistance, float rayInterval)
    {
        this.owner = owner;
        this.rayDistance = rayDistance;
        this.rayInterval = rayInterval;
        rayCoroutine = CoShootRay();
    }

    public IEnumerator CoShootRay()
    {
        var interval = new WaitForSeconds(rayInterval);
        RaycastHit hitData;

        while (true)
        {
            GenerateMuzzleEffect(transform);
            Debug.DrawRay(transform.position, transform.forward.normalized * rayDistance, Color.blue, 0.2f);

            if (Physics.Raycast(transform.position, transform.forward.normalized, out hitData, rayDistance, LayerMask.GetMask("Terrain")))
                CollideWithTerrain(hitData);

            if (Physics.Raycast(transform.position, transform.forward.normalized, out hitData, rayDistance, LayerMask.GetMask("Player")))
                CollideWithPlayer(hitData);

            yield return interval;
        }
    }

    public virtual void GenerateMuzzleEffect(Transform muzzle) { }
    public virtual void CollideWithTerrain(RaycastHit hitData) { }
    public virtual void CollideWithPlayer(RaycastHit hitData) { }

    #region Property
    public IEnumerator RayCoroutine { get { return rayCoroutine; } }
    #endregion
}
