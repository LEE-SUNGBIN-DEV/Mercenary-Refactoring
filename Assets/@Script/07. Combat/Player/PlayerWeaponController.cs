using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController
{
    private PlayerCharacter character;
    private PlayerWeapon currentWeapon;
    private Dictionary<WEAPON_TYPE, PlayerWeapon> weaponDictionary;

    public void Initialize(PlayerCharacter character)
    {
        this.character = character;
        weaponDictionary = new Dictionary<WEAPON_TYPE, PlayerWeapon>();

        if(character.TryGetComponent(out PlayerHalberd halberd))
        {
            weaponDictionary.Add(WEAPON_TYPE.HALBERD, halberd);
            halberd.InitializeWeapon(character);
        }
        if (character.TryGetComponent(out PlayerSwordShield swordShield))
        {
            weaponDictionary.Add(WEAPON_TYPE.SWORD_SHIELD, swordShield);
            swordShield.InitializeWeapon(character);
        }

        weaponDictionary.TryGetValue(character.Status.CurrentWeapon, out currentWeapon);
    }

    public void HideWeapon()
    {
        if (currentWeapon != null)
            currentWeapon.HideWeapon();
    }

    public void ShowWeapon()
    {
        if (currentWeapon != null)
            currentWeapon.ShowWeapon();
    }

    public void SwitchWeapon(WEAPON_TYPE targetWeapon)
    {
        if(weaponDictionary.ContainsKey(targetWeapon))
        {
            if(currentWeapon != null)
                currentWeapon.UnequipWeapon();

            currentWeapon = weaponDictionary[targetWeapon];
            currentWeapon.EquipWeapon();
            character.Status.CurrentWeapon = currentWeapon.WeaponType;
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
