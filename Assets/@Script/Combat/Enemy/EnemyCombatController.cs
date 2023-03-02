using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCombatController : BaseCombatController
{
    [Header("Enemy Combat Controller")]
    [SerializeField] protected BaseEnemy owner;

    protected virtual void ExecuteAttackProcess(Collider other)
    {
        // Hit With Character
        if (other.TryGetComponent(out BaseCharacter character))
        {
            // 01. Invincibility Process
            if (character.IsInvincible)
                return;

            // 02. Prevent Duplicate Damage Process
            if (hitDictionary.ContainsKey(character))
                return;

            hitDictionary.Add(character, true);

            // 03. Damage Process
            owner.DamageProcess(character, damageRatio);

            // 04. Hit Process
            switch (combatType)
            {
                case COMBAT_TYPE.ATTACK_NORMAL:
                    break;

                case COMBAT_TYPE.ATTACK_LIGHT:
                    if (character.Status.HitLevel < (int)COMBAT_TYPE.ATTACK_LIGHT)
                        character.OnLightHit();
                    break;

                case COMBAT_TYPE.ATTACK_HEAVY:
                    if (character.Status.HitLevel < (int)COMBAT_TYPE.ATTACK_HEAVY)
                        character.OnHeavyHit();
                    break;

                case COMBAT_TYPE.ATTACK_STUN:
                    character.OnStun(crowdControlDuration);
                    break;

                default:
                    break;
            }
        }

        // Hit With Shield
        if (other.TryGetComponent(out PlayerDefenseController shield))
            shield.ExecuteDefenseProcess(this, other.ClosestPoint(other.transform.position));
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
