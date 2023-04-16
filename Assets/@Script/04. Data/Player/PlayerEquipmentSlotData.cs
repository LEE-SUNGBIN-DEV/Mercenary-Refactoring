using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerEquipmentSlotData
{
    public event UnityAction<PlayerEquipmentSlotData> OnChangeEquipmentSlotData;
    
    [Header("Equipment Slot")]
    [SerializeField] private ItemData weaponSlotItemData;
    [SerializeField] private ItemData helmetSlotItemData;
    [SerializeField] private ItemData armorSlotItemData;
    [SerializeField] private ItemData bootsSlotItemData;

    public void Initialize()
    {
        weaponSlotItemData = null;
        helmetSlotItemData = null;
        armorSlotItemData = null;
        bootsSlotItemData = null;
    }

    #region Equipment Slot Function
    public ItemData EquipWeaponData(ItemData requestWeaponItemData)
    {
        if (requestWeaponItemData == null)
            return null;

        ItemData equippedItemData = weaponSlotItemData;
        weaponSlotItemData = requestWeaponItemData;
        OnChangeEquipmentSlotData?.Invoke(this);

        return equippedItemData;
    }
    public ItemData UnEquipWeaponData()
    {
        ItemData equippedItemData = weaponSlotItemData;
        weaponSlotItemData = null;
        OnChangeEquipmentSlotData?.Invoke(this);

        return equippedItemData;
    }
    public ItemData EquipHelmetData(ItemData requestWeaponItemData)
    {
        if (requestWeaponItemData == null)
            return null;

        ItemData equippedItemData = helmetSlotItemData;
        helmetSlotItemData = requestWeaponItemData;
        OnChangeEquipmentSlotData?.Invoke(this);

        return equippedItemData;
    }
    public ItemData UnEquipHelmetData()
    {
        ItemData equippedItemData = helmetSlotItemData;
        helmetSlotItemData = null;
        OnChangeEquipmentSlotData?.Invoke(this);

        return equippedItemData;
    }
    public ItemData EquipArmorData(ItemData requestWeaponItemData)
    {
        if (requestWeaponItemData == null)
            return null;

        ItemData equippedItemData = armorSlotItemData;
        armorSlotItemData = requestWeaponItemData;
        OnChangeEquipmentSlotData?.Invoke(this);

        return equippedItemData;
    }
    public ItemData UnEquipArmorData()
    {
        ItemData equippedItemData = armorSlotItemData;
        armorSlotItemData = null;
        OnChangeEquipmentSlotData?.Invoke(this);

        return equippedItemData;
    }
    public ItemData EquipBootsData(ItemData requestWeaponItemData)
    {
        if (requestWeaponItemData == null)
            return null;

        ItemData equippedItemData = bootsSlotItemData;
        bootsSlotItemData = requestWeaponItemData;
        OnChangeEquipmentSlotData?.Invoke(this);

        return equippedItemData;
    }
    public ItemData UnEquipBootsData()
    {
        ItemData equippedItemData = bootsSlotItemData;
        bootsSlotItemData = null;
        OnChangeEquipmentSlotData?.Invoke(this);

        return equippedItemData;
    }
    #endregion

    #region Property
    public ItemData WeaponSlotItemData
    {
        get { return weaponSlotItemData; }
        set
        {
            weaponSlotItemData = value;
            OnChangeEquipmentSlotData?.Invoke(this);
        }
    }
    public ItemData HelmetSlotItemData
    {
        get { return helmetSlotItemData; }
        set
        {
            helmetSlotItemData = value;
            OnChangeEquipmentSlotData?.Invoke(this);
        }
    }
    public ItemData ArmorSlotItemData
    {
        get { return armorSlotItemData; }
        set
        {
            armorSlotItemData = value;
            OnChangeEquipmentSlotData?.Invoke(this);
        }
    }
    public ItemData BootsSlotItemData
    {
        get { return bootsSlotItemData; }
        set
        {
            bootsSlotItemData = value;
            OnChangeEquipmentSlotData?.Invoke(this);
        }
    }
    #endregion
}
