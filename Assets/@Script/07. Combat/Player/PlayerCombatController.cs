using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombatController : BaseCombatController
{
    public event UnityAction OnHitting;

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

        // Counter Attack
        if (character.State.MainState is HalberdCounter
            && other.TryGetComponent(out EnemyCompeteAttack competeAttack)
            && Managers.SpecialCombatManager.TryCounter(this, competeAttack))
        {
            return;
        }

        if (other.TryGetComponent(out EnemyHitbox hitbox))
        {
            Vector3 hitPoint = other.bounds.ClosestPoint(transform.position);

            if (hitbox.Enemy != null)
            {
                // 01. Prevent Duplicate Damage Process
                if (hitDictionary.ContainsKey(hitbox.Enemy))
                    return;

                hitDictionary.Add(hitbox.Enemy, true);

                // 02. Generate Hit VFX Process
                GameObject vfxObject = character.ObjectPooler.RequestObject(Constants.VFX_Player_Attack);
                vfxObject.transform.position = hitPoint;

                // 03. Damage Process
                hitbox.Enemy.TakeHit(character, damageRatio, hitType, hitPoint, crowdControlDuration);
                OnHitting?.Invoke();
            }
        }
    }

    public virtual void ExecuteDefenseProcess(EnemyCombatController enemyCombatController, Vector3 hitPoint)
    {
        character.transform.forward = Functions.GetZeroYDirection(character.transform.position, enemyCombatController.Enemy.transform.position);
        character.PlayerCamera.ActiveCorrectionMode(enemyCombatController.Enemy.transform.position);

        switch (guardType)
        {
            case GUARD_TYPE.NONE:
                return;

            case GUARD_TYPE.GUARDABLE:
                {
                    character.State.SetState(character.CurrentWeapon.GuardBreakState, STATE_SWITCH_BY.WEIGHT);
                    GameObject vfxObject = character.ObjectPooler.RequestObject(Constants.VFX_Player_Guard);
                    vfxObject.transform.position = hitPoint;
                    break;
                }

            case GUARD_TYPE.PARRYABLE:
                {
                    if (character.CurrentWeapon.WeaponType == WEAPON_TYPE.SWORD_SHIELD
                        && enemyCombatController is EnemyCompeteAttack competeAttack
                        && Managers.SpecialCombatManager.TryCompete(this, competeAttack))
                    {
                        break;
                    }

                    character.State.SetState(character.CurrentWeapon.ParryingState, STATE_SWITCH_BY.WEIGHT);
                    GameObject vfxObject = character.ObjectPooler.RequestObject(Constants.VFX_Player_Parrying);
                    vfxObject.transform.position = hitPoint;
                    break;
                }
        }
        combatCollider.enabled = false;
    }

    public PlayerCharacter Character { get { return character; } }
}
