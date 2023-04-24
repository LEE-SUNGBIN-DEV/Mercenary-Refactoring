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
        if (hitType == HIT_TYPE.NONE)
            return;

        if (other.TryGetComponent(out EnemyHitBox hitbox))
        {
            Vector3 hitPoint = other.bounds.ClosestPoint(transform.position);

            if (hitbox.Owner != null)
            {
                // 01. Prevent Duplicate Damage Process
                if (hitDictionary.ContainsKey(hitbox.Owner))
                    return;

                hitDictionary.Add(hitbox.Owner, true);

                // 02. Hitting Effect Process
                GameObject effect = character.ObjectPooler.RequestObject(Constants.VFX_Player_Attack);
                effect.transform.position = hitPoint;

                // 04. Damage Process
                hitbox.Owner.TakeHit(character, damageRatio, hitType, hitPoint, crowdControlDuration);
            }
        }
    }

    public virtual void ExecuteDefenseProcess(EnemyCombatController enemyCombatController, Vector3 hitPoint)
    {
        if (guardType == GUARD_TYPE.NONE)
            return;

        GameObject effect = null;

        switch (guardType)
        {
            case GUARD_TYPE.GUARDABLE:
                {
                    effect = character.ObjectPooler.RequestObject(Constants.VFX_Player_Defense);
                    character.State.SetState(character.CurrentWeapon.GuardBreakState, STATE_SWITCH_BY.WEIGHT);
                    break;
                }

            case GUARD_TYPE.PARRYABLE:
                {
                    if (enemyCombatController is EnemyCompeteAttack competeController && Managers.CompeteManager.TryCompete(this, competeController))
                        break;

                    effect = character.ObjectPooler.RequestObject(Constants.VFX_Player_Parrying);
                    character.State.SetState(character.CurrentWeapon.ParryingState, STATE_SWITCH_BY.WEIGHT);
                    break;
                }
        }

        if (effect != null)
            effect.transform.position = hitPoint;

        combatCollider.enabled = false;
    }

    public PlayerCharacter Character { get { return character; } }
}
