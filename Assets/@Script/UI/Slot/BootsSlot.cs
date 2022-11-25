using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class BootsSlot : EquipmentSlot<BootsItem>
{
    public override void EndDrag()
    {
        if (EndSlot is InventorySlot endInventorySlot)
        {
            if (endInventorySlot.Item is BootsItem)
                InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipBootsData(InventoryData.InventoryItems[EndSlot.SlotIndex]), EndSlot.SlotIndex);

            else if (endInventorySlot.Item == null)
                InventoryData.AddItemDataByIndex(EquipmentSlotData.UnEquipBootsData(), EndSlot.SlotIndex);
        }
    }
}
