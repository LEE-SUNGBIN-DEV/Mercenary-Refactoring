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
    [SerializeField] private ItemData[] inventoryItems;

    [Header("Quick Slot")]
    [SerializeField] private int[] quickSlotItemIDs;

    public InventoryData()
    {
        money = Constants.CHARACTER_DATA_DEFAULT_MONEY;
        inventoryItems = new ItemData[Constants.MAX_INVENTORY_SLOT_NUMBER];

        quickSlotItemIDs = new int[Constants.MAX_QUICK_SLOT_NUMBER];
        for (int i = 0; i < quickSlotItemIDs.Length; ++i)
        {
            quickSlotItemIDs[i] = Constants.NULL_INT;
        }
    }

    #region Inventory Function
    public bool AddOrCombineItem<T>(T item, int count = 1) where T: BaseItem
    {
        if (item is CountItem countableItem)
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
            inventoryItems[emptySlotIndex] = new ItemData(item);
            OnChangeInventoryData?.Invoke(this);
            return true;
        }
        return false;
    }
    public void AddItemToSlotIndex<T>(T item, int slotIndex) where T: BaseItem
    {
        if(item != null)
        {
            inventoryItems[slotIndex] = new ItemData(item);
            OnChangeInventoryData?.Invoke(this);
        }
    }
    public void SwapOrCombineItem(InventorySlot startSlot, InventorySlot endSlot)
    {
        // Try Combine
        if (startSlot.Item is CountItem countItem && endSlot.Item is CountItem)
        {
            if (inventoryItems[startSlot.SlotIndex].itemID == inventoryItems[endSlot.SlotIndex].itemID)
            {
                inventoryItems[endSlot.SlotIndex].itemCount += inventoryItems[endSlot.SlotIndex].itemCount;
                inventoryItems[startSlot.SlotIndex] = null;
                OnChangeInventoryData?.Invoke(this);
                return;
            }
        }
        // Swap
        ItemData temporaryData = inventoryItems[endSlot.SlotIndex];
        inventoryItems[endSlot.SlotIndex] = inventoryItems[startSlot.SlotIndex];
        inventoryItems[startSlot.SlotIndex] = temporaryData;
        OnChangeInventoryData?.Invoke(this);
    }
    public void DestroyItemByIndex(int slotIndex)
    {
        inventoryItems[slotIndex] = null;
        OnChangeInventoryData?.Invoke(this);
    }
    public void RemoveItemByIndex(int slotIndex, int amount = 1)
    {
        inventoryItems[slotIndex].itemCount -= amount;
        if (inventoryItems[slotIndex].itemCount <= 0)
        {
            DestroyItemByIndex(slotIndex);
        }
        OnChangeInventoryData?.Invoke(this);
    }
    public void UseItemByItemID(int itemID, int amount = 1)
    {
        if (Managers.DataManager.ItemTable[itemID] is IUsableItem usableItem)
        {
            usableItem.UseItem(Managers.DataManager.SelectCharacterData.StatusData);
            RemoveItemByIndex(FindItemSlotIndexByID(itemID), amount);
        }
    }
    public void UseQuickSlotItem(int slotIndex, int amount = 1)
    {
        if (quickSlotItemIDs[slotIndex] != Constants.NULL_INT)
        {
            UseItemByItemID(quickSlotItemIDs[slotIndex], amount);
        }
    }
    public void BuyItem(StoreSlot storeSlot)
    {
        if(storeSlot.Item is IShopableItem shopableItem)
        {
            if (CheckMoney(shopableItem.ItemPrice))
            {
                if (AddOrCombineItem(storeSlot.Item))
                {
                    money -= shopableItem.ItemPrice;
                }
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
    public int FindItemSlotIndexByID(int itemID)
    {
        for (int i = 0; i < inventoryItems.Length; ++i)
        {
            if (inventoryItems[i] != null && inventoryItems[i].itemID == itemID)
            {
                return i;
            }
        }
        return Constants.NULL_INT;
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
    public void RegisterQuickSlot(int quickSlotIndex, int itemID)
    {
        quickSlotItemIDs[quickSlotIndex] = itemID;
        OnChangeInventoryData?.Invoke(this);
    }
    public void ReleaseQuickSlot(int quickSlotIndex)
    {
        quickSlotItemIDs[quickSlotIndex] = Constants.NULL_INT;
        OnChangeInventoryData?.Invoke(this);
    }
    public void SwapQuickSlot(int startSlotIndex, int endSlotIndex)
    {
        int temporaryID = quickSlotItemIDs[startSlotIndex];
        quickSlotItemIDs[startSlotIndex] = quickSlotItemIDs[endSlotIndex];
        quickSlotItemIDs[endSlotIndex] = temporaryID;
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
    public ItemData[] InventoryItems
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
