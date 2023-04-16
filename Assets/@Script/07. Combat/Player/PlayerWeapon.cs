using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
    protected PlayerCharacter character;
    protected WEAPON_TYPE weaponType;
    protected Dictionary<COMBAT_ACTION_TYPE, CombatActionInfomation> combatInformationDictionary;

    protected ACTION_STATE idleState;
    protected ACTION_STATE walkState;
    protected ACTION_STATE runState;
    protected ACTION_STATE guardBreakState;
    protected ACTION_STATE parryingState;

    public abstract void InitializeWeapon(PlayerCharacter character);
    public abstract void AddStateToCharacter(StateController state);
    public abstract void AddCombatInformation();
    public abstract void AddCommonStateInforamtion();
    public abstract void EquipWeapon();
    public abstract void UnequipWeapon();

    #region Property
    public WEAPON_TYPE WeaponType { get { return weaponType; } }
    public Dictionary<COMBAT_ACTION_TYPE, CombatActionInfomation> CombatInformationDictionary { get { return combatInformationDictionary; } }
    public ACTION_STATE IdleState { get { return idleState; } }
    public ACTION_STATE WalkState { get { return WalkState; } }
    public ACTION_STATE RunState { get { return runState; } }
    public ACTION_STATE GuardBreakState { get { return guardBreakState; } }
    public ACTION_STATE ParryingState { get { return parryingState; } }
    #endregion
}
