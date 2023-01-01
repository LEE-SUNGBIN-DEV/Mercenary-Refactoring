using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCombatController : BaseCombatController
{
    [Header("Enemy Combat Controller")]
    [SerializeField] protected BaseEnemy owner;

    protected virtual void ExecuteAttackProcess(Collider target)
    {
        // Hit With Character
        if (target.TryGetComponent(out BaseCharacter character))
        {
            // Invincible
            if (character.IsInvincible)
            {
                return;
            }

            // Duplication Check
            if (hitDictionary.ContainsKey(character))
                return;

            hitDictionary.Add(character, true);

            // Damage Process
            owner.DamageProcess(character, damageRatio);

            // CC Process
            switch (abnormalType)
            {
                case ABNORMAL_TYPE.Stun:
                    character.OnStun(abnormalStateDuration);
                    break;

                default:
                    break;
            }

            // Hit Process
            switch (combatType)
            {
                case HIT_TYPE.Light:
                    if (character is ILightHitable lightHitableObject)
                        lightHitableObject.OnLightHit();
                    break;
                case HIT_TYPE.Heavy:
                    if (character is IHeavyHitable heavyHitableObject)
                        heavyHitableObject.OnHeavyHit();
                    break;
            }
        }

        // Hit With Shield
        if (target.TryGetComponent(out PlayerShield shield))
            shield.DefenseProcess(this, target.ClosestPoint(shield.transform.position));
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
