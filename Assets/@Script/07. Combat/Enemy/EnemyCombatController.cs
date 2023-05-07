using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyCombatController : BaseCombatController
{
    public event UnityAction OnHitPlayer;
    public event UnityAction OnHitPlayerGuard;

    [Header("Enemy Combat Controller")]
    [SerializeField] protected BaseEnemy enemy;

    protected virtual void ExecuteAttackProcess(Collider other)
    {
        // Hit With Character
        if (other.TryGetComponent(out PlayerCharacter character))
        {
            // Prevent Duplicate Damage Process
            if (hitDictionary.ContainsKey(character))
                return;

            hitDictionary.Add(character, true);

            character.TakeHit(enemy, damageRatio, hitType, crowdControlDuration);
            OnHitPlayer?.Invoke();
        }

        // Hit With Shield
        if (other.TryGetComponent(out PlayerCombatController weapon))
        {
            if (hitDictionary.ContainsKey(weapon))
                return;

            switch (weapon.GuardType)
            {
                case GUARD_TYPE.GUARDABLE:
                case GUARD_TYPE.PARRYABLE:
                    weapon.ExecuteDefenseProcess(this, other.ClosestPoint(other.transform.position));
                    OnHitPlayerGuard?.Invoke();
                    break;

                default:
                    break;
            }
        }
    }

    public virtual void OnEnableCollider()
    {
        if(combatCollider != null && combatCollider.enabled == false)
            combatCollider.enabled = true;
    }
    public virtual void OnDisableCollider()
    {
        if(combatCollider != null && combatCollider.enabled == true)
        {
            combatCollider.enabled = false;
            hitDictionary.Clear();
        }
    }

    #region Property
    public BaseEnemy Enemy { get { return enemy; } set { enemy = value; } }
    #endregion
}
