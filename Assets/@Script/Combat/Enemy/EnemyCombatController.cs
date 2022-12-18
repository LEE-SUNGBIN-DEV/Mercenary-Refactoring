using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCombatController : BaseCombatController
{
    [Header("Enemy Combat Controller")]
    [SerializeField] protected BaseEnemy owner;

    protected virtual void ExecuteAttackProcess(Collider target)
    {
        if (target.TryGetComponent(out BaseCharacter character))
        {
            if (character.IsInvincible)
            {
                return;
            }

            owner.DamageProcess(character, damageRatio);

            // CC Process
            switch (crowdControlType)
            {
                case ABNORMAL_STATE.Stun:
                    character.OnStun(7f);
                    break;

                default:
                    break;
            }

            // Hit Process
            switch (combatType)
            {
                case HIT_TYPE.Light:
                    character.OnHit();
                    break;
                case HIT_TYPE.Heavy:
                    character.OnHeavyHit();
                    break;
            }            
        }
    }

    #region Property
    public BaseEnemy Owner { get { return owner; } set { owner = value; } }
    #endregion
}
