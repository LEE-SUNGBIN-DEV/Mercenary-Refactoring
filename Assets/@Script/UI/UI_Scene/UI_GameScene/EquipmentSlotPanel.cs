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
    }

    public void LoadFromCharacterData<T, U>(T loadItem, U targetSlot) where T: EquipmentItem where U: EquipmentSlot
    {
    }

    #region Property
    public WeaponSlot WeaponSlot { get { return weaponSlot; } set { weaponSlot = value; } }
    public HelmetSlot HelmetSlot { get { return helmetSlot; } set { helmetSlot = value; } }
    public ArmorSlot ArmorSlot { get { return armorSlot; } set { armorSlot = value; } }
    public BootsSlot BootsSlot { get { return bootsSlot; } set { bootsSlot = value; } }

    #endregion
}
