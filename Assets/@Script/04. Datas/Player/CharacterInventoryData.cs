using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;

[System.Serializable]
public class CharacterInventoryData
{
    public event UnityAction<CharacterInventoryData> OnChangeInventoryData;
    public event UnityAction<CharacterInventoryData> OnChangeCharacterWeapon;

    [Header("Inventory")]
    [SerializeField] private int resonanceStone;
    [SerializeField] private ItemData[] inventoryItems;

    [Header("Resonance Water")]
    [SerializeField] private int resonanceWaterGrade;
    [SerializeField] private int resonanceWaterMaxCount;
    [SerializeField] private int resonanceWaterRemainingCount;
    [SerializeField] private float resonanceWaterRecoverAmount;

    [Header("Equipment Slot")]
    [SerializeField] private WEAPON_TYPE equippedWeapon;
    [SerializeField] private int halberdID;
    [SerializeField] private int swordShieldID;
    [SerializeField] private int armorID;

    [Header("Quick Slot")]
    [SerializeField] private int[] quickSlotItemIDs;

    public void CreateData()
    {
        equippedWeapon = WEAPON_TYPE.HALBERD;

        resonanceStone = Constants.CHARACTER_DATA_DEFAULT_STONE;
        inventoryItems = new ItemData[Constants.MAX_INVENTORY_SLOT_NUMBER];

        resonanceWaterGrade = 0;
        RefillResonanceWater();

        halberdID = 0;
        swordShieldID = 0;
        armorID = 0;

        quickSlotItemIDs = new int[Constants.MAX_QUICK_SLOT_NUMBER];
        for (int i = 0; i < quickSlotItemIDs.Length; ++i)
        {
            quickSlotItemIDs[i] = Constants.NULL_INT;
        }

        OnChangeInventoryData?.Invoke(this);
    }

    #region Resonance Water Functions
    public bool TryDrinkResonanceWater()
    {
        if (resonanceWaterRemainingCount > 0)
            return true;

        else
            return false;
    }

    public void DrinkResonanceWater()
    {
        resonanceWaterRemainingCount--;
        OnChangeInventoryData?.Invoke(this);
    }

    public void AutoSetResonanceWaterByGrade()
    {
        resonanceWaterMaxCount = 3 + resonanceWaterGrade;
        resonanceWaterRecoverAmount = 20 + resonanceWaterGrade;
        OnChangeInventoryData?.Invoke(this);
    }

    public void RefillResonanceWater()
    {
        AutoSetResonanceWaterByGrade();
        resonanceWaterRemainingCount = resonanceWaterMaxCount;
        OnChangeInventoryData?.Invoke(this);
    }

    public float GetRemainingResonanceWaterRatio()
    {
        return (float)resonanceWaterRemainingCount / resonanceWaterMaxCount;
    }
    #endregion

    #region Equipment Function
    public void EquipWeapon(CharacterStatusData statusData, WEAPON_TYPE weaponType)
    {
        equippedWeapon = weaponType;
        switch (weaponType)
        {
            case WEAPON_TYPE.HALBERD:
                statusData.EquipWeapon(this, WEAPON_TYPE.HALBERD);
                break;
            case WEAPON_TYPE.SWORD_SHIELD:
                statusData.EquipWeapon(this, WEAPON_TYPE.SWORD_SHIELD);
                break;
        }

        OnChangeInventoryData?.Invoke(this);
    }
    public void EquipArmor(CharacterStatusData statusData)
    {
        statusData.EquipArmor(this);
        OnChangeInventoryData?.Invoke(this);
    }

    public void UpgradeWeapons()
    {

    }
    public void UpgradeArmor()
    {

    }
    #endregion

    #region Inventory Function
    public void DestroyItemByIndex(int slotIndex)
    {
        inventoryItems[slotIndex] = null;
        OnChangeInventoryData?.Invoke(this);
    }
    public bool AddItem<T>(T item, int count = 1) where T : BaseItem
    {
        if (item == null)
            return false;

        if (item is CountItem countableItem)
        {
            for (int i = 0; i < inventoryItems.Length; ++i)
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
    public void AddItemByIndex<T>(T item, int slotIndex) where T : BaseItem
    {
        if (item != null)
        {
            inventoryItems[slotIndex] = new ItemData(item);
            OnChangeInventoryData?.Invoke(this);
        }
        else
            DestroyItemByIndex(slotIndex);
    }
    public void AddItemDataByIndex(ItemData itemData, int slotIndex)
    {
        inventoryItems[slotIndex] = itemData;
        OnChangeInventoryData?.Invoke(this);
    }
    public void SwapOrCombineSlotItem(InventorySlot startSlot, InventorySlot endSlot)
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
        (inventoryItems[startSlot.SlotIndex], inventoryItems[endSlot.SlotIndex]) = (inventoryItems[endSlot.SlotIndex], inventoryItems[startSlot.SlotIndex]);
        OnChangeInventoryData?.Invoke(this);
    }
    public void RemoveItemByIndex(int slotIndex, int amount = 1)
    {
        inventoryItems[slotIndex].itemCount -= amount;
        if (inventoryItems[slotIndex].itemCount <= 0)
            DestroyItemByIndex(slotIndex);

        OnChangeInventoryData?.Invoke(this);
    }
    public void UseItemByItemID(int itemID, int amount = 1)
    {
        if (Managers.DataManager.ItemTable[itemID] is IUsableItem usableItem)
        {
            usableItem.UseItem(Managers.DataManager.CurrentCharacterData.StatusData);
            RemoveItemByIndex(FindSlotIndexByItemID(itemID), amount);
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
        if (storeSlot.Item is IShopableItem shopableItem)
        {
            if (CheckResonanceStone(shopableItem.ItemPrice))
            {
                if (AddItem(storeSlot.Item))
                {
                    resonanceStone -= shopableItem.ItemPrice;
                }
            }
        }
    }
    public bool CheckResonanceStone(int price)
    {
        if (resonanceStone < price)
        {
            Debug.Log("공명석이 부족합니다.");
            return false;
        }
        else
            return true;
    }
    public int FindSlotIndexByItemID(int itemID)
    {
        for (int i = 0; i < inventoryItems.Length; ++i)
        {
            if (inventoryItems[i] != null && inventoryItems[i].itemID == itemID)
                return i;
        }
        Debug.Log("아이템이 존재하지 않습니다.");
        return Constants.NULL_INT;
    }
    public int FindEmptySlotIndex()
    {
        for (int i = 0; i < inventoryItems.Length; ++i)
        {
            if (inventoryItems[i] == null)
                return i;
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
    public WEAPON_TYPE EquippedWeapon { get { return equippedWeapon; } set { equippedWeapon = value; OnChangeCharacterWeapon?.Invoke(this); } }
    public int ResonanceStone
    {
        get { return resonanceStone; }
        set
        {
            resonanceStone = value;
            if (resonanceStone < 0)
                resonanceStone = 0;
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

    public int ResonanceWaterGrade
    {
        get { return resonanceWaterGrade; }
        set
        {
            resonanceWaterGrade = value;

            if (resonanceWaterGrade < 0)
                resonanceWaterGrade = 0;

            AutoSetResonanceWaterByGrade();
            OnChangeInventoryData?.Invoke(this);
        }
    }

    public int ResonanceWaterMaxCount
    {
        get { return resonanceWaterMaxCount; }
        set
        {
            resonanceWaterMaxCount = value;

            if (resonanceWaterMaxCount < 0)
                resonanceWaterMaxCount = 1;

            OnChangeInventoryData?.Invoke(this);
        }
    }

    public int ResonanceWaterRemainingCount
    {
        get
        { return resonanceWaterRemainingCount; }
        set
        {
            resonanceWaterRemainingCount = value;

            if (resonanceWaterRemainingCount < 0)
                resonanceWaterRemainingCount = 0;

            if (resonanceWaterRemainingCount > resonanceWaterMaxCount)
                resonanceWaterRemainingCount = resonanceWaterMaxCount;

            OnChangeInventoryData?.Invoke(this);
        }
    }
    [JsonIgnore]
    public float ResonanceWaterRecoverAmount
    {
        get
        {
            return resonanceWaterRecoverAmount;
        }
    }
    public int HalberdID
    {
        get { return halberdID; }
        set
        {
            halberdID = value;
            OnChangeInventoryData?.Invoke(this);
        }
    }
    public int SwordShieldID
    {
        get { return swordShieldID; }
        set
        {
            swordShieldID = value;
            OnChangeInventoryData?.Invoke(this);
        }
    }
    public int ArmorID
    {
        get { return armorID; }
        set
        {
            armorID = value;
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
