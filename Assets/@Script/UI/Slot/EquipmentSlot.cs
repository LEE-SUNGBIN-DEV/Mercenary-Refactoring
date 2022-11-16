using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EquipmentSlot : BaseSlot
{
    [Header("Equipment Slot")]
    protected bool isEquip;

    public override void Initialize()
    {
        base.Initialize();
        isEquip = false;
    }

    public void EquipItem<T>() where T : BaseItem
    {
        EquipItem(item);
    }
    public abstract void EquipItem<T>(T item) where T: BaseItem;
    public abstract void ReleaseItem();

    public bool IsEquip { get { return isEquip; } }
}
