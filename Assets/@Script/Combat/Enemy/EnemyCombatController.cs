using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : BaseCombatController
{
    [Header("Enemy Combat Controller")]
    [SerializeField] protected float rayDistance;
    [SerializeField] protected float rayInterval;
    protected Vector3 rotationOffset;
    protected BaseEnemy owner;
    protected IEnumerator rayCoroutine;

    public virtual void Initialize(BaseEnemy owner)
    {
        if (isInitialized == false)
        {
            base.Initialize();
            this.owner = owner;
            rayCoroutine = ShootRay();
            isInitialized = true;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other != null)
            ExecuteCombatProcess(other);
    }

    public virtual void ExecuteCombatProcess(Collider target)
    {
        if (target.TryGetComponent(out BaseCharacter character))
        {
            if (character.IsInvincible)
            {
                return;
            }

            owner.DamageProcess(character, damageRatio);
            switch (combatType)
            {
                case HIT_TYPE.Light:
                    {
                        character.OnHit();
                        break;
                    }
                case HIT_TYPE.Heavy:
                    {
                        character.OnHeavyHit();
                        break;
                    }
            }
        }
    }

    public void SetRay(float rayDistance, float rayInterval)
    {
        this.rayDistance = rayDistance;
        this.rayInterval = rayInterval;
    }

    public IEnumerator ShootRay()
    {
        RaycastHit hitData;
        var interval = new WaitForSeconds(rayInterval);

        while(true)
        {
            GenerateMuzzleEffect(transform);
            Debug.DrawRay(transform.position, transform.forward.normalized * rayDistance, Color.blue, 0.2f);

            if(Physics.Raycast(transform.position, transform.forward.normalized, out hitData, rayDistance, LayerMask.GetMask("Terrain")))
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
    public BaseEnemy Owner { get { return owner; } set { owner = value; } }
    public IEnumerator RayCoroutine { get { return rayCoroutine; } }
    #endregion
}
