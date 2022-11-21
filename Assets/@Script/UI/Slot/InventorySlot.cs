using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : BaseSlot
{
    [SerializeField] private BaseItem item;

    public void Initialize(int i)
    {
        base.Initialize();
        slotIndex = i;
    }

    public override void LoadSlot(ItemData itemData)
    {
        if (itemData != null)
        {
            BaseItem item = Managers.DataManager.ItemTable[itemData.itemID];
            SetSlotByItem(item);
        }
    }

    public override void SlotRightClick(PointerEventData eventData)
    {
    }

    public override void Drop()
    {
        // Target Slot is Inventory

        if (Managers.SlotManager.SelectSlot is InventorySlot) // Inventory -> Inventory
        {
            // Swap or Combine
        }
        else if(Managers.SlotManager.SelectSlot is WeaponSlot) // WeaponSlot -> Inventory
        {
            // Swap or UnEquip
        }
        else if (Managers.SlotManager.SelectSlot is HelmetSlot) // HelmetSlot -> Inventory
        {
            // Swap or UnEquip
        }
        else if (Managers.SlotManager.SelectSlot is ArmorSlot) // ArmorSlot -> Inventory
        {
            // Swap or UnEquip
        }
        else if (Managers.SlotManager.SelectSlot is BootsSlot) // BootsSlot -> inventory
        {
            // Swap or UnEquip
        }
        else if (Managers.SlotManager.SelectSlot is QuickSlot) // QuickSlot -> Inventory
        {
            // Release
        }
    }


    public void EndDragEquipmentSlot<T, U>() where T: EquipmentSlot where U: EquipmentItem
    {
        if(Managers.SlotManager.TargetSlot is T)
        {
            if (item is not U)
            {
                return;
            }
            else
            {
                //Equip
            }
        }
    }
    public void EndDragQuickSlot<T, U>() where T: QuickSlot where U: IUsableItem
    {
        if (Managers.SlotManager.TargetSlot is T)
        {
            if (item is not U)
            {
                return;
            }
            else
            {
                //Register
            }
        }
    }
    public override void EndDrag()
    {
        // Select Slot is Inventory

        if (Managers.SlotManager.TargetSlot is InventorySlot)
        {
            // Swap or Combine
        }
        EndDragEquipmentSlot<WeaponSlot, WeaponItem>();
        EndDragEquipmentSlot<HelmetSlot, HelmetItem>();
        EndDragEquipmentSlot<ArmorSlot, ArmorItem>();
        EndDragEquipmentSlot<BootsSlot, BootsItem>();
        EndDragQuickSlot<QuickSlot, IUsableItem>();
    }
}
