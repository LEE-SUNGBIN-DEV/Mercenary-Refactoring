using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class ArmorSlot : EquipmentSlot<ArmorItem>
{
    public override void EndDrag()
    {
        if (EndSlot is InventorySlot endInventorySlot)
        {
            if (endInventorySlot.Item is ArmorItem)
                InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipArmorData(InventoryData.InventoryItems[EndSlot.SlotIndex]), EndSlot.SlotIndex);

            else if (endInventorySlot.Item == null)
                InventoryData.AddItemDataByIndex(EquipmentSlotData.UnEquipArmorData(), EndSlot.SlotIndex);
        }
    }
}
