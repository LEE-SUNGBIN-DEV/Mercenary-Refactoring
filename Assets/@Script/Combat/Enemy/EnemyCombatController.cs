using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCombatController : BaseCombatController
{
    [Header("Enemy Combat Controller")]
    [SerializeField] protected BaseEnemy owner;

    protected virtual void ExecuteAttackProcess(Collider target)
    {
        if (target.TryGetComponent(out BaseCharacter playerCharacter))
        {
            if (playerCharacter.IsInvincible)
            {
                return;
            }

            if (hitDictionary.ContainsKey(playerCharacter))
                return;

            hitDictionary.Add(playerCharacter, true);

            owner.DamageProcess(playerCharacter, damageRatio);

            // CC Process
            switch (abnormalType)
            {
                case ABNORMAL_TYPE.Stun:
                    playerCharacter.OnStun(abnormalStateDuration);
                    break;

                default:
                    break;
            }

            // Hit Process
            switch (combatType)
            {
                case HIT_TYPE.Light:
                    playerCharacter.OnHit();
                    break;
                case HIT_TYPE.Heavy:
                    playerCharacter.OnHeavyHit();
                    break;
            }
        }

        if (target.TryGetComponent(out PlayerShield playerDefense))
        {

        }
    }

    public virtual void OnEnableCollider()
    {
        if(combatCollider != null)
            combatCollider.enabled = true;
    }
    public virtual void OnDisableCollider()
    {
        if(combatCollider != null)
        {
            combatCollider.enabled = false;
            hitDictionary.Clear();
        }
    }

    #region Property
    public BaseEnemy Owner { get { return owner; } set { owner = value; } }
    #endregion
}
