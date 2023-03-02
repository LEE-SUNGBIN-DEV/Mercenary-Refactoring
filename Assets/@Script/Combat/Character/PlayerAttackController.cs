using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttackController : BaseCombatController
{
    [Header("Player Attack")]
    protected BaseCharacter owner;

    public virtual void SetWeapon(BaseCharacter character)
    {
        owner = character;
        owner.ObjectPooler.RegisterObject(Constants.VFX_Player_Attack, 12);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
            ExecuteAttackProcess(other);
    }

    public virtual void ExecuteAttackProcess(Collider other)
    {
        if (other.TryGetComponent(out EnemyHitBox hitbox))
        {
            Vector3 hitPoint = other.bounds.ClosestPoint(transform.position);

            if (hitbox.Owner != null)
            {
                // 01. Invincibility Process
                if (hitbox.Owner.IsInvincible)
                    return;

                // 02. Prevent Duplicate Damage Process
                if (hitDictionary.ContainsKey(hitbox.Owner))
                    return;

                hitDictionary.Add(hitbox.Owner, true);

                // 03. Hitting Effect Process
                GameObject effect = owner.ObjectPooler.RequestObject(Constants.VFX_Player_Attack);
                effect.transform.position = hitPoint;

                // 04. Damage Process
                owner.DamageProcess(hitbox.Owner, damageRatio, hitPoint);

                // 05. Hit Process
                switch (combatType)
                {
                    case COMBAT_TYPE.ATTACK_NORMAL:
                        break;
                    case COMBAT_TYPE.ATTACK_LIGHT:
                        if (hitbox.Owner.Status.HitLevel < (int)COMBAT_TYPE.ATTACK_LIGHT)
                            hitbox.Owner.OnLightHit();
                        break;

                    case COMBAT_TYPE.ATTACK_HEAVY:
                        if (hitbox.Owner.Status.HitLevel < (int)COMBAT_TYPE.ATTACK_HEAVY)
                            hitbox.Owner.OnHeavyHit();
                        break;
                    case COMBAT_TYPE.ATTACK_STUN:
                        if (hitbox.Owner is IStunable stunableObject)
                            stunableObject.OnStun(crowdControlDuration);
                        break;

                        default:
                        break;
                }
            }
        }
    }

    public BaseCharacter Owner { get { return owner; } }
}
