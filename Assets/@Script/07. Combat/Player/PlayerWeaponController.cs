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

        weaponDictionary.TryGetValue(character.InventoryData.EquippedWeapon, out currentWeapon);
    }

    public void HideWeapon(bool isHide)
    {
        if (currentWeapon == null)
            return;

        switch (isHide)
        {
            case true:
                currentWeapon.HideWeapon();
                break;

            case false:
                currentWeapon.ShowWeapon();
                break;
        }
    }

    public void SwitchWeapon(WEAPON_TYPE targetWeapon)
    {
        if(weaponDictionary.ContainsKey(targetWeapon))
        {
            if(currentWeapon != null)
            {
                currentWeapon.UnequipWeapon();
            }

            currentWeapon = weaponDictionary[targetWeapon];
            currentWeapon.EquipWeapon();
            character.InventoryData.EquipWeapon(character.Status, currentWeapon.WeaponType);
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
