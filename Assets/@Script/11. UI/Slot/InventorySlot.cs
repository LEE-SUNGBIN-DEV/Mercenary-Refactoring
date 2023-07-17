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
        if (itemData != null && Managers.DataManager.ItemTable.ContainsKey(itemData.itemID))
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

        /*
        else if (EndSlot is RuneSlot && Item is RuneItem)
            InventoryData.AddItemDataByIndex(EquipmentSlotData.EquipWeaponData(InventoryData.InventoryItems[this.slotIndex]), this.slotIndex);
        */
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
