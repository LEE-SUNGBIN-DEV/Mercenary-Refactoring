using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class HelmetSlot : EquipmentSlot
{
    [SerializeField] private HelmetItem item;

    public override void UnEquipItem()
    {

    }
    public override void LoadSlot(ItemData itemSaveData)
    {
        if (itemSaveData != null)
        {
            HelmetItem item = Managers.DataManager.ItemTable[itemSaveData.itemID] as HelmetItem;
            SetSlotByItem(item);
        }
    }
    public override void SetSlotByItem<T>(T requestItem)
    {
        base.SetSlotByItem(requestItem);
    }

    public override void Drop()
    {
        DropEquipmentSlot<HelmetItem>();
    }

    public override void EndDrag()
    {
        EndDragEquipmentSlot<HelmetItem>();
    }

    public override void SlotRightClick(PointerEventData eventData)
    {
        UnEquipItem();
    }
}
