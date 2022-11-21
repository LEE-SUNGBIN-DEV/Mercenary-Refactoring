using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class ArmorSlot : EquipmentSlot
{
    [SerializeField] private ArmorItem item;

    public override void UnEquipItem()
    {
    }
    public override void LoadSlot(ItemData itemSaveData)
    {
        if (itemSaveData != null)
        {
            ArmorItem item = Managers.DataManager.ItemTable[itemSaveData.itemID] as ArmorItem;
            SetSlotByItem(item);
        }
    }
    public override void Drop()
    {
        DropEquipmentSlot<ArmorItem>();
    }

    public override void EndDrag()
    {
        EndDragEquipmentSlot<ArmorItem>();
    }

    public override void SlotRightClick(PointerEventData eventData)
    {
        UnEquipItem();
    }
}
