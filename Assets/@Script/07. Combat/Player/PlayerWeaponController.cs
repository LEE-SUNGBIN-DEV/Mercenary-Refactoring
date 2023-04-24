using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private PlayerWeapon currentWeapon;
    private Dictionary<WEAPON_TYPE, PlayerWeapon> weaponDictionary;

    public void Initialize(PlayerCharacter character)
    {
        weaponDictionary = new Dictionary<WEAPON_TYPE, PlayerWeapon>()
        {
            { WEAPON_TYPE.HALBERD, new PlayerHalberd() },
            { WEAPON_TYPE.SWORD_SHIELD, new PlayerSwordShield() }
        };

        foreach (var weapon in weaponDictionary.Values)
        {
            weapon.InitializeWeapon(character);
        }
        currentWeapon = weaponDictionary[WEAPON_TYPE.HALBERD];
    }

    public void SwitchWeapon(WEAPON_TYPE targetWeapon)
    {
        if(weaponDictionary.ContainsKey(targetWeapon))
        {
            currentWeapon?.UnequipWeapon();
            currentWeapon = weaponDictionary[targetWeapon];
            currentWeapon.EquipWeapon();
        }
    }

    public T GetWeapon<T>(WEAPON_TYPE targetWeapon) where T : PlayerWeapon
    {
        if (weaponDictionary.TryGetValue(targetWeapon, out PlayerWeapon weapon) && weapon is T)
        {
            return weapon as T;
        }

        return null;
    }

    public PlayerWeapon CurrentWeapon { get { return currentWeapon; } }
}
