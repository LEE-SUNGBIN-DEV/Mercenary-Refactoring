using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotPanel : UIPanel
{
    [SerializeField] private WeaponSlot weaponSlot;
    [SerializeField] private HelmetSlot helmetSlot;
    [SerializeField] private ArmorSlot armorSlot;
    [SerializeField] private BootsSlot bootsSlot;

    public override void Initialize()
    {
        weaponSlot = GetComponentInChildren<WeaponSlot>();
        helmetSlot = GetComponentInChildren<HelmetSlot>();
        armorSlot = GetComponentInChildren<ArmorSlot>();
        bootsSlot = GetComponentInChildren<BootsSlot>();
    }

    private void OnEnable()
    {
        LoadFromCharacterData(Managers.DataManager.CurrentCharacterData.WeaponSlotItem, weaponSlot);
        LoadFromCharacterData(Managers.DataManager.CurrentCharacterData.HelmetSlotItem, helmetSlot);
        LoadFromCharacterData(Managers.DataManager.CurrentCharacterData.ArmorSlotItem, armorSlot);
        LoadFromCharacterData(Managers.DataManager.CurrentCharacterData.BootsSlotItem, bootsSlot);
    }

    public void LoadFromCharacterData<T, U>(T loadItem, U targetSlot) where T: EquipmentItem where U: EquipmentSlot
    {
        if (loadItem != null)
        {
            targetSlot.AddItemToSlot(loadItem);
        }
        else
        {
            targetSlot.ClearSlot();
        }
    }

    #region Property
    public WeaponSlot WeaponSlot { get { return weaponSlot; } set { weaponSlot = value; } }
    public HelmetSlot HelmetSlot { get { return helmetSlot; } set { helmetSlot = value; } }
    public ArmorSlot ArmorSlot { get { return armorSlot; } set { armorSlot = value; } }
    public BootsSlot BootsSlot { get { return bootsSlot; } set { bootsSlot = value; } }

    #endregion
}
