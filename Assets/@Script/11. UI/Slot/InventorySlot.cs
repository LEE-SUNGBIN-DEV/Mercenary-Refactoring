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

    public void LoadSlot(ItemData itemData)
    {
        ClearSlot();
        if (itemData != null)
        {
            item = Managers.DataManager.ItemTable[itemData.itemID];
            itemCount = itemData.itemCount;

            if (item != null)
            {
                itemImage.sprite = item.ItemSprite;
                itemImage.color = Functions.SetColor(Color.white, 1f);
                if (item is CountItem)
                {
                    itemCount = itemData.itemCount;
                    EnableCountText(true);
                }
                else if (item is EquipmentItem)
                {
                    itemGrade = itemData.grade;
                    EnableGradeText(true);
                }
                else
                {
                    EnableGradeText(false);
                    EnableCountText(false);
                }
            }
        }
    }
    public void DestroyItem() { }
    public override void EndDrag()
    {
        if (EndSlot == null)
            DestroyItem();

        else if (EndSlot is InventorySlot endInventorySlot)
            InventoryData.SwapOrCombineSlotItem(this, endInventorySlot);

        else if (EndSlot is QuickSlot endQuickSlot)
            InventoryData.RegisterQuickSlot(endQuickSlot.SlotIndex, this.Item.ItemID);

        else if (EndSlot is WeaponSlot && this.Item is WeaponItem)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipWeaponData(InventoryData.InventoryItems[this.slotIndex]), this.slotIndex);

        else if (EndSlot is HelmetSlot && this.Item is HelmetItem)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipHelmetData(InventoryData.InventoryItems[this.slotIndex]), this.slotIndex);

        else if (EndSlot is ArmorSlot && this.Item is ArmorItem)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipArmorData(InventoryData.InventoryItems[this.slotIndex]), this.slotIndex);

        else if (EndSlot is BootsSlot && this.Item is BootsItem)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipBootsData(InventoryData.InventoryItems[this.slotIndex]), this.slotIndex);
    }
    public override void ClearSlot()
    {
        base.ClearSlot();
        item = null;
    }

    public override void SlotRightClick(PointerEventData eventData)
    {
    }

    public BaseItem Item { get { return item; } }
}
