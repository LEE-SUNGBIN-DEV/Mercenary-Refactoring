using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EquipmentSlotData
{
    public event UnityAction<EquipmentSlotData> OnChangeEquipmentSlotData;
    
    [Header("Equipment Slot")]
    [SerializeField] private ItemData weaponSlotItemData;
    [SerializeField] private ItemData helmetSlotItemData;
    [SerializeField] private ItemData armorSlotItemData;
    [SerializeField] private ItemData bootsSlotItemData;

    public EquipmentSlotData()
    {
        weaponSlotItemData = null;
        helmetSlotItemData = null;
        armorSlotItemData = null;
        bootsSlotItemData = null;
    }

    #region Equipment Slot Function
    public ItemData EquipWeapon(WeaponItem requestWeaponItem)
    {
        if (requestWeaponItem == null) return null;

        ItemData equippedItemData = null;
        if (weaponSlotItemData != null)
        {
            equippedItemData = weaponSlotItemData;
        }
        ItemData requestWeaponItemData = new ItemData(requestWeaponItem);
        weaponSlotItemData = requestWeaponItemData;
        OnChangeEquipmentSlotData?.Invoke(this);

        return equippedItemData;
    }
    public void UnEquipItem()
    {
        if (weaponSlotItemData != null)
        {
            weaponSlotItemData = null;
            OnChangeEquipmentSlotData?.Invoke(this);
        }
    }
    public void EquipHelmet(HelmetItem helmetItem)
    {
        helmetSlotItemData = new ItemData(helmetItem);
    }
    public void ReleaseHelmet()
    {
        helmetSlotItemData = null;
    }
    public void EquipArmor(ArmorItem armorItem)
    {
        armorSlotItemData = new ItemData(armorItem);
    }
    public void ReleaseArmor()
    {
        armorSlotItemData = null;
    }
    public void EquipBoots(BootsItem bootsItem)
    {
        bootsSlotItemData = new ItemData(bootsItem);
    }
    public void ReleaseBoots()
    {
        bootsSlotItemData = null;
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
