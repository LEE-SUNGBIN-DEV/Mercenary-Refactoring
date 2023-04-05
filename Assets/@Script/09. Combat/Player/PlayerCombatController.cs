using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : BaseCombatController
{
    [Header("Player Weapon")]
    protected PlayerCharacter character;

    public virtual void Initialize(PlayerCharacter character)
    {
        base.Initialize();
        this.character = character;
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
                GameObject effect = character.ObjectPooler.RequestObject(Constants.VFX_Player_Attack);
                effect.transform.position = hitPoint;

                // 04. Damage Process
                character.DamageProcess(hitbox.Owner, damageRatio, hitPoint);

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

    public virtual void ExecuteDefenseProcess(EnemyCombatController enemyCombatController, Vector3 hitPoint)
    {
        GameObject effect = null;

        switch (combatType)
        {
            case COMBAT_TYPE.GUARDABLE:
                {
                    effect = character.ObjectPooler.RequestObject(Constants.VFX_Player_Defense);
                    character.State.SetState(ACTION_STATE.PLAYER_HALBERD_GUARD_BREAK, STATE_SWITCH_BY.WEIGHT);
                    break;
                }

            case COMBAT_TYPE.PARRYABLE:
                {
                    if (enemyCombatController is EnemyCompeteAttack competeController && Managers.CompeteManager.TryCompete(this, competeController))
                        break;

                    effect = character.ObjectPooler.RequestObject(Constants.VFX_Player_Parrying);
                    character.State.SetState(ACTION_STATE.PLAYER_HALBERD_PARRYING, STATE_SWITCH_BY.WEIGHT);
                    break;
                }
        }

        if (effect != null)
            effect.transform.position = hitPoint;

        combatCollider.enabled = false;
    }

    public PlayerCharacter Character { get { return character; } }
}
