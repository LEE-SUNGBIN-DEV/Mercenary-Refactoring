using GSpawn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueEquipmentController
{
    private CharacterInventoryData inventoryData;

    [Header("Weapons")]
    private Dictionary<WEAPON_TYPE, PlayerWeapon> weaponDictionary;
    private PlayerWeapon currentWeapon;

    [Header("Response Water")]
    private ResponseWater responseWater;

    public void Initialize(PlayerCharacter character)
    {
        inventoryData = character.InventoryData;

        weaponDictionary = new Dictionary<WEAPON_TYPE, PlayerWeapon>();
        if (character.TryGetComponent(out PlayerHalberd halberd))
        {
            weaponDictionary.Add(WEAPON_TYPE.HALBERD, halberd);
            halberd.Initialize(character);
        }
        if (character.TryGetComponent(out PlayerSwordShield swordShield))
        {
            weaponDictionary.Add(WEAPON_TYPE.SWORD_SHIELD, swordShield);
            swordShield.Initialize(character);
        }
        weaponDictionary.TryGetValue(inventoryData.CurrentWeaponType, out currentWeapon);

        responseWater = character.GetComponentInChildren<ResponseWater>(true);
        responseWater.Initialize();
        inventoryData.OnChangeInventoryData -= UpdateResponseWater;
        inventoryData.OnChangeInventoryData += UpdateResponseWater;
        UpdateResponseWater(inventoryData);
    }

    public void ShowWeapon()
    {
        if (currentWeapon != null)
            currentWeapon.ShowWeapon();
    }
    public void HideWeapon()
    {
        if (currentWeapon != null)
            currentWeapon.HideWeapon();
    }
    public void ShowResponseWater()
    {
        HideWeapon();
        responseWater.ShowResponseWater();
    }
    public void HideResponseWater()
    {
        responseWater.HideResponseWater();
        ShowWeapon();
    }

    public void UpdateResponseWater(CharacterInventoryData inventoryData)
    {
        responseWater.SetFillRatio(inventoryData.GetRemainingResponseWaterRatio());
    }


    public void SwitchWeapon(WEAPON_TYPE targetWeapon)
    {
        if(weaponDictionary.ContainsKey(targetWeapon))
        {
            if(currentWeapon != null)
                currentWeapon.UnequipWeapon();

            currentWeapon = weaponDictionary[targetWeapon];
            currentWeapon.EquipWeapon();

            // Data
            inventoryData.SwitchWeapon(currentWeapon.WeaponType);
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
    public ResponseWater ResponseWater { get { return responseWater; } }
}
