using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EquipmentSlotData
{
    public event UnityAction<EquipmentSlotData> OnChangeEquipmentSlotData;
    
    [Header("Equipment Slot")]
    [SerializeField] private ItemSaveData weaponSlotItem;
    [SerializeField] private ItemSaveData helmetSlotItem;
    [SerializeField] private ItemSaveData armorSlotItem;
    [SerializeField] private ItemSaveData bootsSlotItem;

    public EquipmentSlotData()
    {
        weaponSlotItem = null;
        helmetSlotItem = null;
        armorSlotItem = null;
        bootsSlotItem = null;
    }

    #region Equipment Slot Function
    public void EquipWeapon(WeaponItem weaponItem)
    {
        WeaponSlotItem = new ItemSaveData(weaponItem);
    }
    public void ReleaseWeapon()
    {
        WeaponSlotItem = null;
    }
    public void EquipHelmet(HelmetItem helmetItem)
    {
        HelmetSlotItem = new ItemSaveData(helmetItem);
    }
    public void ReleaseHelmet()
    {
        HelmetSlotItem = null;
    }
    public void EquipArmor(ArmorItem armorItem)
    {
        ArmorSlotItem = new ItemSaveData(armorItem);
    }
    public void ReleaseArmor()
    {
        ArmorSlotItem = null;
    }
    public void EquipBoots(BootsItem bootsItem)
    {
        BootsSlotItem = new ItemSaveData(bootsItem);
    }
    public void ReleaseBoots()
    {
        BootsSlotItem = null;
    }
    #endregion

    #region Property
    public ItemSaveData WeaponSlotItem
    {
        get { return weaponSlotItem; }
        set
        {
            weaponSlotItem = value;
            OnChangeEquipmentSlotData?.Invoke(this);
        }
    }
    public ItemSaveData HelmetSlotItem
    {
        get { return helmetSlotItem; }
        set
        {
            helmetSlotItem = value;
            OnChangeEquipmentSlotData?.Invoke(this);
        }
    }
    public ItemSaveData ArmorSlotItem
    {
        get { return armorSlotItem; }
        set
        {
            armorSlotItem = value;
            OnChangeEquipmentSlotData?.Invoke(this);
        }
    }
    public ItemSaveData BootsSlotItem
    {
        get { return bootsSlotItem; }
        set
        {
            bootsSlotItem = value;
            OnChangeEquipmentSlotData?.Invoke(this);
        }
    }
    #endregion
}
