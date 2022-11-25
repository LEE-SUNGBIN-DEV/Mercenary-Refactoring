using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class HelmetSlot : EquipmentSlot<HelmetItem>
{
    public override void EndDrag()
    {
        if (EndSlot is InventorySlot endInventorySlot)
        {
            if (endInventorySlot.Item is HelmetItem)
                InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipHelmetData(InventoryData.InventoryItems[EndSlot.SlotIndex]), EndSlot.SlotIndex);

            else if (endInventorySlot.Item == null)
                InventoryData.AddItemDataByIndex(EquipmentSlotData.UnEquipHelmetData(), EndSlot.SlotIndex);
        }
    }
}
