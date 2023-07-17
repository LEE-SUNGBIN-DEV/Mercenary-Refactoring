using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading;
using Cysharp.Threading.Tasks;

public abstract class PlayerWeapon : MonoBehaviour
{
    protected PlayerCharacter character;

    protected PlayerCombatController attackController;
    protected PlayerCombatController guardController;

    protected WEAPON_TYPE weaponType;
    protected MeshRenderer[] weaponRenderers;
    protected MaterialController materialController;
    protected Dictionary<COMBAT_ACTION_TYPE, CombatControllerInfomation> weaponCombatTable;

    protected ACTION_STATE idleState;
    protected ACTION_STATE walkState;
    protected ACTION_STATE runState;
    protected ACTION_STATE guardBreakState;
    protected ACTION_STATE parryingState;

    protected Coroutine phaseCoroutine;

    public virtual void InitializeWeapon(PlayerCharacter character)
    {
        this.character = character;
    }
    public abstract void AddWeaponState(StateController state);
    public abstract void AddCombatTable();
    public abstract void AddCommonStateInforamtion();
    public abstract void EquipWeapon();
    public abstract void UnequipWeapon();
    public abstract void PlayWeaponHittingSFX();
    public void ShowWeapon(float duration = 0.5f)
    {
        if (phaseCoroutine != null)
            StopCoroutine(phaseCoroutine);

        phaseCoroutine = StartCoroutine(materialController.CoDissolve(1, 0, duration, materialController.SetDefaultMaterials));
    }
    public void HideWeapon(float duration = 0.5f, UnityAction callback = null)
    {
        if (phaseCoroutine != null)
            StopCoroutine(phaseCoroutine);

        phaseCoroutine = StartCoroutine(materialController.CoDissolve(0, 1, duration, callback));
    }

    #region Property
    public PlayerCombatController AttackController { get { return attackController; } }
    public PlayerCombatController GuardController { get { return guardController; } }
    public WEAPON_TYPE WeaponType { get { return weaponType; } }
    public Dictionary<COMBAT_ACTION_TYPE, CombatControllerInfomation> WeaponCombatTable { get { return weaponCombatTable; } }
    public ACTION_STATE IdleState { get { return idleState; } }
    public ACTION_STATE WalkState { get { return walkState; } }
    public ACTION_STATE RunState { get { return runState; } }
    public ACTION_STATE GuardBreakState { get { return guardBreakState; } }
    public ACTION_STATE ParryingState { get { return parryingState; } }
    #endregion
}
