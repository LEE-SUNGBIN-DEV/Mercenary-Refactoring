using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class WeaponSlot : EquipmentSlot<WeaponItem>
{
    public override void EndDrag()
    {
        if (EndSlot is InventorySlot endInventorySlot)
        {
            if (endInventorySlot.Item is WeaponItem)
                InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipWeaponData(InventoryData.InventoryItems[EndSlot.SlotIndex]), EndSlot.SlotIndex);

            else if (endInventorySlot.Item == null)
                InventoryData.AddItemDataByIndex(EquipmentSlotData.UnEquipWeaponData(), EndSlot.SlotIndex);
        }
    }
}
