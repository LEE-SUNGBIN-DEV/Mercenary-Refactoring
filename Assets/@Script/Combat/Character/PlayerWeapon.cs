using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeapon : BaseCombatController
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
            ExecuteCombatProcess(other);
    }

    public virtual void ExecuteCombatProcess(Collider other)
    {
        Vector3 hitPoint = other.bounds.ClosestPoint(transform.position);
        AttackProcess(other, hitPoint);
    }

    public void AttackProcess(Collider other, Vector3 hitPoint)
    {
        if (other.TryGetComponent(out EnemyHitBox hitbox))
        {
            if(hitbox.Owner != null)
            {
                // 01 Invincibility Process
                if (hitbox.Owner.IsInvincible)
                    return;

                // 02 Prevent Duplicate Damage Process
                if (hitDictionary.ContainsKey(hitbox.Owner))
                    return;

                hitDictionary.Add(hitbox.Owner, true);
                
                // 03 Hitting Effect Process
                GameObject effect = owner.ObjectPooler.RequestObject(Constants.VFX_Player_Attack);
                effect.transform.position = hitPoint;

                // 04 Damage Process
                owner.DamageProcess(hitbox.Owner, damageRatio, hitPoint);

                // 05 Hit Process
                switch (combatType)
                {
                    case HIT_TYPE.Light:
                        hitbox.Owner.OnLightHit();
                        break;
                    case HIT_TYPE.Heavy:
                        hitbox.Owner.OnHeavyHit();
                        break;
                }

                // 06 CC Process
                switch (abnormalType)
                {
                    case ABNORMAL_TYPE.None:
                        break;
                    case ABNORMAL_TYPE.Stun:
                        hitbox.Owner.OnStun();
                        break;
                }
            }
        }
    }

    public BaseCharacter Owner { get { return owner; } }
}
