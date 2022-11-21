using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class WeaponSlot : EquipmentSlot
{
    [SerializeField] private WeaponItem item;

    public override void UnEquipItem()
    {

    }
    public override void LoadSlot(ItemData itemSaveData)
    {
        if (itemSaveData != null)
        {
            WeaponItem item = Managers.DataManager.ItemTable[itemSaveData.itemID] as WeaponItem;
            SetSlotByItem(item);
        }
    }
    public override void Drop()
    {
        DropEquipmentSlot<WeaponItem>();
    }

    public override void EndDrag()
    {
        EndDragEquipmentSlot<WeaponItem>();
    }

    public override void SlotRightClick(PointerEventData eventData)
    {
        UnEquipItem();
    }
}
