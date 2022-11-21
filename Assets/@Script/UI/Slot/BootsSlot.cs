using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class BootsSlot : EquipmentSlot
{
    [SerializeField] private BootsItem item;

    public override void UnEquipItem()
    {
    }
    public override void LoadSlot(ItemData itemSaveData)
    {
        if (itemSaveData != null)
        {
            BootsItem item = Managers.DataManager.ItemTable[itemSaveData.itemID] as BootsItem;
            SetSlotByItem(item);
        }
    }
    public override void Drop()
    {
        DropEquipmentSlot<BootsItem>();
    }

    public override void EndDrag()
    {
        EndDragEquipmentSlot<BootsItem>();
    }

    public override void SlotRightClick(PointerEventData eventData)
    {
        UnEquipItem();
    }
}
