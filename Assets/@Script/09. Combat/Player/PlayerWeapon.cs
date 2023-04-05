using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
    protected PlayerCharacter character;
    protected WEAPON_TYPE weaponType;
    protected Dictionary<COMBAT_ACTION_TYPE, CombatActionInfomation> combatInformationDictionary;
    protected BasicStateInformation basicStateInformation;

    public abstract void InitializeWeapon(PlayerCharacter character);
    public abstract void AddStateToCharacter(StateController state);
    public abstract void AddCombatInformation();
    public abstract void AddCommonStateInforamtion();
    public abstract void EquipWeapon();
    public abstract void UnequipWeapon();

    #region Property
    public WEAPON_TYPE WeaponType { get { return weaponType; } }
    public Dictionary<COMBAT_ACTION_TYPE, CombatActionInfomation> CombatInformationDictionary { get { return combatInformationDictionary; } }
    public BasicStateInformation BasicStateInformation { get { return basicStateInformation; } }
    #endregion
}
