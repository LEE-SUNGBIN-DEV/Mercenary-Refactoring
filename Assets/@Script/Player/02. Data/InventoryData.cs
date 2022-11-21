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
    public bool AddItem<T>(T item, int count = 1) where T: BaseItem
    {
        if (item is IUsableItem countableItem)
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
    public void AddItemBySlot<T>(T slot) where T: BaseSlot
    {
        AddItemByIndex(Managers.DataManager.ItemTable[inventoryItems[slot.SlotIndex].itemID], slot.SlotIndex);
    }
    public void AddItemByIndex<T>(T item, int slotIndex) where T : BaseItem
    {
        if(item is CountItem countItem)
        {
            if (inventoryItems[slotIndex].itemID == item.ItemID)
            {
                inventoryItems[slotIndex].itemCount += countItem.ItemCount;
                OnChangeInventoryData?.Invoke(this);
                return;
            }
        }
        else
        {
            inventoryItems[slotIndex] = new ItemData(item);
            OnChangeInventoryData?.Invoke(this);
            return;
        }
    }
    public void ExchangeItem(BaseSlot selectSlot, BaseSlot targetSlot)
    {
        AddItemBySlot(targetSlot);
        AddItemBySlot(selectSlot);
    }
    public void CombineItem(BaseSlot selectSlot, BaseSlot targetSlot)
    {
        AddItemBySlot(targetSlot);
        DestroyItem(selectSlot.SlotIndex);
    }
    public void RemoveItem(int slotIndex, int count = 1)
    {
        inventoryItems[slotIndex].itemCount -= count;
        if (inventoryItems[slotIndex].itemCount <= 0)
        {
            inventoryItems[slotIndex] = null;
        }
        OnChangeInventoryData?.Invoke(this);
        return;
    }
    public void DestroyItem(int slotIndex)
    {
        inventoryItems[slotIndex] = null;
        OnChangeInventoryData?.Invoke(this);
        return;
    }
    public void BuyItem(StoreSlot storeSlot)
    {
        if(storeSlot.Item is IShopableItem shopableItem)
        {
            if (CheckMoney(shopableItem.ItemPrice))
            {
                if (AddItem(storeSlot.Item))
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
