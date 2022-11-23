using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class QuickSlot : BaseSlot
{
    [SerializeField] private IUsableItem item;

    public void Initialize(int i)
    {
        base.Initialize();
        slotIndex = i;
    }

    public void LoadSlot(InventoryData inventoryData)
    {
        ClearSlot();
        if (inventoryData.QuickSlotItemIDs[slotIndex] != Constants.NULL_INT)
        {
            BaseItem quickSlotItem = Managers.DataManager.ItemTable[inventoryData.QuickSlotItemIDs[slotIndex]];
            item = quickSlotItem as IUsableItem;
            if (item != null)
            {
                itemImage.sprite = quickSlotItem.ItemSprite;
                itemImage.color = Functions.SetColor(Color.white, 1f);

                if (quickSlotItem is CountItem)
                {
                    itemCount = 0;
                    for (int i = 0; i < inventoryData.InventoryItems.Length; ++i)
                    {
                        if (inventoryData.InventoryItems[i] != null && inventoryData.QuickSlotItemIDs[slotIndex] == inventoryData.InventoryItems[i].itemID)
                        {
                            itemCount += inventoryData.InventoryItems[i].itemCount;
                        }
                    }
                    if (itemCount > 0)
                        itemImage.color = Color.white;
                    else
                        itemImage.color = Color.gray;

                    EnableCountText(true);
                }
                else
                {
                    itemImage.color = Color.white;
                    EnableCountText(false);
                }
            }
        }
    }

    public void UseItem(InventoryData inventoryData)
    {
        inventoryData.UseQuickSlotItem(slotIndex);
    }

    public void ReleaseQuickSlot()
    {
        Managers.DataManager.SelectCharacterData.InventoryData.ReleaseQuickSlot(slotIndex);
    }

    #region Mouse Event Function
    public override void SlotRightClick(PointerEventData eventData)
    {
        ReleaseQuickSlot();
    }
    #endregion

    public IUsableItem Item { get { return item; } }
}
