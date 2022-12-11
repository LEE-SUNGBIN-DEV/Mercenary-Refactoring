using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : BaseCombatController
{
    [Header("Enemy Combat Controller")]
    [SerializeField] private float rayDistance;
    [SerializeField] private float rayInterval;
    protected Vector3 rotationOffset;
    protected BaseEnemy owner;
    protected IEnumerator rayCoroutine;

    public virtual void Initialize(BaseEnemy owner)
    {
        base.Initialize();
        this.owner = owner;
        rayCoroutine = ShootRay();
    }

    private void OnTriggerEnter(Collider other)
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
                case COMBAT_TYPE.EnemyNormalAttack:
                    {
                        character.OnHit();
                        break;
                    }
                case COMBAT_TYPE.EnemySmashAttack:
                    {
                        character.OnHeavyHit();
                        break;
                    }
                case COMBAT_TYPE.EnemyCounterableAttack:
                case COMBAT_TYPE.EnemyCompetableAttack:
                case COMBAT_TYPE.StunAttack:
                    {
                        character.OnStun();
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
        Ray ray = new Ray(transform.position, transform.forward.normalized);
        var interval = new WaitForSeconds(rayInterval);

        while(true)
        {
            Debug.DrawRay(transform.position, transform.forward.normalized * rayDistance, Color.blue, 0.1f);
            if (Physics.Raycast(ray, out RaycastHit hitData, rayDistance))
            {
                if (hitData.transform.gameObject.layer == LayerMask.GetMask("Terrain"))
                    CollideWithTerrain(hitData);

                if (hitData.transform.gameObject.layer == LayerMask.GetMask("Player"))
                    CollideWithPlayer(hitData);
            }
            yield return interval;
        }
    }
    public virtual void CollideWithTerrain(RaycastHit hitData) { }
    public virtual void CollideWithPlayer(RaycastHit hitData) { }

    #region Property
    public BaseEnemy Owner { get { return owner; } set { owner = value; } }
    public IEnumerator RayCoroutine { get { return rayCoroutine; } }
    #endregion
}
