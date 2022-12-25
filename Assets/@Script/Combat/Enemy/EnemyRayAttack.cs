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
        float time = 0f;
        while (true)
        {
            GenerateMuzzleEffect(transform);
            Debug.DrawRay(transform.position, transform.forward.normalized * rayDistance, Color.blue, 0.1f);
            if (Physics.SphereCast(transform.position, 1f, transform.forward, out RaycastHit hitData, 30f, LayerMask.GetMask("Player")))
            {
                CollideWithPlayer(hitData);
            }

            if ((time >= rayInterval) && Physics.Raycast(transform.position, transform.forward, out hitData, 30f, LayerMask.GetMask("Terrain")))
            {
                CollideWithTerrain(hitData);
                time -= rayInterval;
            }
            
            time += Time.deltaTime;
            yield return null;
        }
    }

    public virtual void GenerateMuzzleEffect(Transform muzzle) { }
    public virtual void CollideWithTerrain(RaycastHit hitData) { }
    public virtual void CollideWithPlayer(RaycastHit hitData) { }

    #region Property
    public IEnumerator RayCoroutine { get { return rayCoroutine; } }
    #endregion
}
