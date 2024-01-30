using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class QuickSlot : BaseItemSlot
{
    public void ConnectData(CharacterInventoryData inventoryData)
    {
        if (!string.IsNullOrEmpty(inventoryData.QuickSlotItemIDs[slotIndex]))
        {
            BaseItem quickSlotItem = inventoryData.FindInventoryItem<BaseItem>(inventoryData.QuickSlotItemIDs[slotIndex]);
            if (quickSlotItem != null)
            {
                itemImage.sprite = quickSlotItem.GetItemSprite();
                itemImage.color = new Color32(255, 255, 255, 255);

                if (quickSlotItem is IStackableItem)
                {
                    itemCount = 0;
                    for (int i = 0; i < inventoryData.InventoryItems.Length; ++i)
                    {
                        if (inventoryData.InventoryItems[i] != null
                            && inventoryData.QuickSlotItemIDs[slotIndex] == inventoryData.InventoryItems[i].GetItemID()
                            && inventoryData.InventoryItems[i] is IStackableItem inventoryStackableItem)
                        {
                            itemCount += inventoryStackableItem.ItemCount;
                        }
                    }
                    ShowAmountText();
                }
                else
                    HideAmountText();
            }
            else
            {
                ItemData itemData = Managers.DataManager.ItemTable[inventoryData.QuickSlotItemIDs[slotIndex]];
                itemImage.sprite = itemData.GetItemSprite();
                itemImage.color = new Color32(255, 255, 255, 255);

                itemImage.color = Color.gray;
                HideAmountText();
            }
        }
    }

    #region Mouse Event Function
    public override void SlotEndDrag()
    {
        if (ToSlot == null)
            inventoryData?.ReleaseQuickSlot(this);

        else if (ToSlot is QuickSlot toQuickSlot)
            inventoryData?.FromQuickSlotToQuickSlot(this, toQuickSlot);
    }
    public override void OnSlotRightClicked(PointerEventData eventData)
    {
        inventoryData?.ReleaseQuickSlot(this);
    }
    #endregion
}
