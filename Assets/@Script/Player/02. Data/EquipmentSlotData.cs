using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EquipmentSlotData
{
    public event UnityAction<EquipmentSlotData> OnChangeEquipmentSlotData;
    
    [Header("Equipment Slot")]
    [SerializeField] private ItemData weaponSlotItem;
    [SerializeField] private ItemData helmetSlotItem;
    [SerializeField] private ItemData armorSlotItem;
    [SerializeField] private ItemData bootsSlotItem;

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
        WeaponSlotItem = new ItemData(weaponItem);
    }
    public void ReleaseWeapon()
    {
        WeaponSlotItem = null;
    }
    public void EquipHelmet(HelmetItem helmetItem)
    {
        HelmetSlotItem = new ItemData(helmetItem);
    }
    public void ReleaseHelmet()
    {
        HelmetSlotItem = null;
    }
    public void EquipArmor(ArmorItem armorItem)
    {
        ArmorSlotItem = new ItemData(armorItem);
    }
    public void ReleaseArmor()
    {
        ArmorSlotItem = null;
    }
    public void EquipBoots(BootsItem bootsItem)
    {
        BootsSlotItem = new ItemData(bootsItem);
    }
    public void ReleaseBoots()
    {
        BootsSlotItem = null;
    }
    #endregion

    #region Property
    public ItemData WeaponSlotItem
    {
        get { return weaponSlotItem; }
        set
        {
            weaponSlotItem = value;
            OnChangeEquipmentSlotData?.Invoke(this);
        }
    }
    public ItemData HelmetSlotItem
    {
        get { return helmetSlotItem; }
        set
        {
            helmetSlotItem = value;
            OnChangeEquipmentSlotData?.Invoke(this);
        }
    }
    public ItemData ArmorSlotItem
    {
        get { return armorSlotItem; }
        set
        {
            armorSlotItem = value;
            OnChangeEquipmentSlotData?.Invoke(this);
        }
    }
    public ItemData BootsSlotItem
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
