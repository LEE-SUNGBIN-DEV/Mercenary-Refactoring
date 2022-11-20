using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class InventoryData
{
    public event UnityAction<InventoryData> OnChangeInventoryData;

    [Header("Inventory")]
    [SerializeField] private int money;
    [SerializeField] private ItemSaveData[] inventoryItems;

    [Header("Quick Slot")]
    [SerializeField] private int[] quickSlotItemIDs;

    public InventoryData()
    {
        money = Constants.CHARACTER_DATA_DEFAULT_MONEY;
        inventoryItems = new ItemSaveData[Constants.MAX_INVENTORY_SLOT_NUMBER];

        quickSlotItemIDs = new int[Constants.MAX_QUICK_SLOT_NUMBER];
        for (int i = 0; i < quickSlotItemIDs.Length; ++i)
        {
            quickSlotItemIDs[i] = Constants.NULL_INT;
        }
    }

    #region Inventory Function
    public bool AddItemToInventory<T>(T item, int count = 1) where T: BaseItem
    {
        if (item.IsCountable)
        {
            for(int i=0; i<inventoryItems.Length; ++i)
            {
                if (inventoryItems[i].itemID == item.ItemID)
                {
                    inventoryItems[i].itemCount += count;
                    OnChangeInventoryData?.Invoke(this);
                    return true;
                }
            }
        }

        int emptySlotIndex = FindEmptySlotIndex();
        if (emptySlotIndex != Constants.NULL_INT)
        {
            inventoryItems[emptySlotIndex] = new ItemSaveData(item);
            OnChangeInventoryData?.Invoke(this);
            return true;
        }

        return false;
    }
    public bool AddItemBySlot(BaseSlot slot)
    {
        return AddItemByIndex(slot.Item, slot.SlotIndex, slot.ItemCount);
    }
    public bool AddItemByIndex<T>(T item, int slotIndex, int count = 1) where T : BaseItem
    {
        if(item != null)
        {
            if (inventoryItems[slotIndex].itemID == item.ItemID)
            {
                if (item.IsCountable)
                {
                    inventoryItems[slotIndex].itemCount += count;
                    OnChangeInventoryData?.Invoke(this);
                    return true;
                }
            }
            Debug.Log($"itemType: {item.ItemType}");
            Debug.Log($"itemID: {item.ItemID}");
            Debug.Log($"itemName: {item.ItemName}");
            Debug.Log($"itemCount: {item.ItemCount}");
            inventoryItems[slotIndex] = new ItemSaveData(item);
            OnChangeInventoryData?.Invoke(this);
            return true;
        }
        else
        {
            Debug.Log($"slotIndex: {slotIndex}");
            inventoryItems[slotIndex] = null;
            OnChangeInventoryData?.Invoke(this);
            return true;
        }
    }
    public void RemoveItemFromInventory(int slotIndex, int count = 1)
    {
        inventoryItems[slotIndex].itemCount -= count;
        if (inventoryItems[slotIndex].itemCount <= 0)
        {
            inventoryItems[slotIndex] = null;
        }
        OnChangeInventoryData?.Invoke(this);
        return;
    }
    public void DestroyItemFromInventory<T>(int slotIndex)
    {
        inventoryItems[slotIndex] = null;
        OnChangeInventoryData?.Invoke(this);
        return;
    }
    public void BuyItem(StoreSlot storeSlot)
    {
        if (CheckMoney(storeSlot.Item.ItemPrice))
        {
            if (AddItemToInventory(storeSlot.Item))
            {
                money -= storeSlot.Item.ItemPrice;
            }
        }
    }
    public bool CheckMoney(int price)
    {
        if (money < price)
        {
            Debug.Log("소지금이 부족합니다.");
            return false;
        }
        else
        {
            return true;
        }
    }
    public int FindEmptySlotIndex()
    {
        for (int i = 0; i < inventoryItems.Length; ++i)
        {
            if (inventoryItems[i] == null)
            {
                return i;
            }
        }
        Debug.Log("인벤토리 공간이 부족합니다.");
        return Constants.NULL_INT;
    }
    #endregion

    #region QuickSlot Function
    public void RegisterQuicklot(int itemID, int slotIndex)
    {
        quickSlotItemIDs[slotIndex] = itemID;
        OnChangeInventoryData?.Invoke(this);
    }
    public void ReleaseQuickSlot(int slotIndex)
    {
        quickSlotItemIDs[slotIndex] = Constants.NULL_INT;
        OnChangeInventoryData?.Invoke(this);
    }
    #endregion

    #region Property
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            if (money < 0)
            {
                money = 0;
            }
            OnChangeInventoryData?.Invoke(this);
        }
    }
    public ItemSaveData[] InventoryItems
    {
        get { return inventoryItems; }
        set
        {
            inventoryItems = value;
            OnChangeInventoryData?.Invoke(this);
        }
    }
    public int[] QuickSlotItemIDs
    {
        get { return quickSlotItemIDs; }
        set
        {
            quickSlotItemIDs = value;
            OnChangeInventoryData?.Invoke(this);
        }
    }
    #endregion
}
