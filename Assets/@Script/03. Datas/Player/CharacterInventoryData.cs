using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using DarkLandsUI.Scripts.Equipment;

[System.Serializable]
public class CharacterInventoryData : ICharacterData
{
    public event UnityAction<CharacterInventoryData> OnChangeInventoryData;
    public event UnityAction<RuneItem> OnEquipRune;
    public event UnityAction<RuneItem> OnUnequipRune;
    public event UnityAction<HalberdItem> OnChangeHalberd;
    public event UnityAction<SwordShieldItem> OnChangeSwordShield;
    public event UnityAction<ArmorItem> OnChangeArmor;
    public event UnityAction<int> OnRewardResponseStone;
    public event UnityAction<BaseItem> OnRewardItem;

    [Header("Save Data")]
    [JsonProperty][SerializeField] private ItemSaveData[] inventorySlotSaveDatas;
    [JsonProperty][SerializeField] private ItemSaveData[] runeSlotSaveDatas;
    [JsonProperty][SerializeField] private ItemSaveData halberdSlotSaveData;
    [JsonProperty][SerializeField] private ItemSaveData swordShieldSlotSaveData;
    [JsonProperty][SerializeField] private ItemSaveData armorSlotSaveData;
    [JsonProperty][SerializeField] private ItemSaveData responseWaterSlotSaveData;
    [JsonProperty][SerializeField] private string[] quickSlotItemIDs;

    [JsonProperty][SerializeField] private int responseStone;
    [JsonProperty][SerializeField] private int responseWaterRemainingCount;
    [JsonProperty][SerializeField] private WEAPON_TYPE currentWeaponType;

    [Header("Runetime Datas")]
    [JsonIgnore][SerializeField] private BaseItem[] inventoryItems;
    [JsonIgnore][SerializeField] private RuneItem[] runeSlotItems;
    [JsonIgnore][SerializeField] private HalberdItem halberdSlotItem;
    [JsonIgnore][SerializeField] private SwordShieldItem swordShieldSlotItem;
    [JsonIgnore][SerializeField] private ArmorItem armorSlotItem;
    [JsonIgnore][SerializeField] private ResponseWaterItem responseWaterSlotItem;

    public void CreateData()
    {
        responseStone = Constants.CHARACTER_DATA_DEFAULT_RESPONSE_STONE;

        inventorySlotSaveDatas = new ItemSaveData[Constants.MAX_INVENTORY_SLOT_COUNTS];
        runeSlotSaveDatas = new ItemSaveData[Constants.MAX_RUNE_SLOT_COUNTS];
        halberdSlotSaveData = new ItemSaveData() { itemID = "ITEM_HALBERD_0", itemCount = 1 };
        swordShieldSlotSaveData = new ItemSaveData() { itemID = "ITEM_SWORD_SHIELD_0", itemCount = 1 };
        armorSlotSaveData = new ItemSaveData() { itemID = "ITEM_ARMOR_0", itemCount = 1 };
        responseWaterSlotSaveData = new ItemSaveData() { itemID = "ITEM_RESPONSE_WATER_0", itemCount = 1 };

        quickSlotItemIDs = new string[Constants.MAX_QUICK_SLOT_COUNTS];

        currentWeaponType = WEAPON_TYPE.HALBERD;
        responseWaterRemainingCount = Managers.DataManager.ResponseWaterTable[responseWaterSlotSaveData.itemID].maxCount;
    }
    public void LoadData()
    {
        inventoryItems = new BaseItem[Constants.MAX_INVENTORY_SLOT_COUNTS];
        for (int i = 0; i < inventoryItems.Length; ++i)
        {
            if (inventorySlotSaveDatas[i]?.itemID != null && Managers.DataManager.ItemTable.TryGetValue(inventorySlotSaveDatas[i].itemID, out ItemData itemTableData))
            {
                switch (itemTableData.itemType)
                {
                    case ITEM_TYPE.NORMAL:
                        inventoryItems[i] = new BaseItem(inventorySlotSaveDatas[i]?.itemID);
                        break;
                    case ITEM_TYPE.RUNE:
                        inventoryItems[i] = new RuneItem(inventorySlotSaveDatas[i]?.itemID);
                        break;
                    default:
                        inventoryItems[i] = null;
                        break;
                }
                inventoryItems[i].LoadFromSaveData(inventorySlotSaveDatas[i]);
            }
            else
                inventoryItems[i] = null;
        }

        halberdSlotItem = new HalberdItem(halberdSlotSaveData.itemID);
        halberdSlotItem.LoadFromSaveData(halberdSlotSaveData);

        swordShieldSlotItem = new SwordShieldItem(swordShieldSlotSaveData.itemID);
        swordShieldSlotItem.LoadFromSaveData(swordShieldSlotSaveData);

        armorSlotItem = new ArmorItem(armorSlotSaveData.itemID);
        armorSlotItem.LoadFromSaveData(armorSlotSaveData);

        responseWaterSlotItem = new ResponseWaterItem(responseWaterSlotSaveData.itemID);
        responseWaterSlotItem.LoadFromSaveData(responseWaterSlotSaveData);

        runeSlotItems = new RuneItem[Constants.MAX_RUNE_SLOT_COUNTS];
        for (int i = 0; i < runeSlotItems.Length; ++i)
        {
            if (runeSlotSaveDatas[i]?.itemID != null)
            {
                runeSlotItems[i] = new RuneItem(runeSlotSaveDatas[i].itemID);
                runeSlotItems[i].LoadFromSaveData(runeSlotSaveDatas[i]);
            }
            else
                runeSlotItems[i] = null;
        }
    }
    public void UpdateData(CharacterData characterData)
    {
        if (characterData.LocationData != null)
        {
            characterData.LocationData.OnChangeLocationData -= UpgradeResponseWater;
            characterData.LocationData.OnChangeLocationData += UpgradeResponseWater;
            UpgradeResponseWater(characterData.LocationData);
        }
    }

    public void SaveData()
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] != null)
            {
                inventorySlotSaveDatas[i] = new ItemSaveData();
                inventorySlotSaveDatas[i].SaveItemData(inventoryItems[i]);
            }
            else
                inventorySlotSaveDatas[i] = null;
        }
        for (int i = 0; i < runeSlotItems.Length; i++)
        {
            if (runeSlotItems[i] != null)
            {
                runeSlotSaveDatas[i] = new ItemSaveData();
                runeSlotSaveDatas[i].SaveItemData(runeSlotItems[i]);
            }
            else
                runeSlotSaveDatas[i] = null;
        }
        halberdSlotSaveData.SaveItemData(halberdSlotItem);
        swordShieldSlotSaveData.SaveItemData(swordShieldSlotItem);
        armorSlotSaveData.SaveItemData(armorSlotItem);
        responseWaterSlotSaveData.SaveItemData(responseWaterSlotItem);
    }

    #region Resonance Water Functions
    public void UpgradeResponseWater(CharacterLocationData locationData)
    {
        while (locationData.UnlockedPointHashSet.Count > responseWaterSlotItem.ResponseWaterData.responseCount)
        {
            if (responseWaterSlotItem.IsMaxGrade())
                return;
            responseWaterSlotItem.UpgradeItem();
        }
        OnChangeInventoryData?.Invoke(this);
    }
    public bool TryConsumeResponseWater()
    {
        return responseWaterRemainingCount > 0;
    }
    public void ConsumeResponseWater(CharacterStatusData statusData)
    {
        responseWaterRemainingCount--;
        responseWaterSlotItem.Consume(statusData);
        OnChangeInventoryData?.Invoke(this);
    }
    public void RefillResponseWater()
    {
        responseWaterRemainingCount = ResponseWaterSlotItem.ResponseWaterData.maxCount;
        OnChangeInventoryData?.Invoke(this);
    }
    public float GetRemainingResponseWaterRatio()
    {
        return (float)responseWaterRemainingCount / ResponseWaterSlotItem.ResponseWaterData.maxCount;
    }
    #endregion

    #region Equipment Function
    public void SwitchWeapon(WEAPON_TYPE weaponType)
    {
        currentWeaponType = weaponType;
        OnChangeInventoryData?.Invoke(this);
    }
    public void UpgradeWeapons(WEAPON_TYPE weaponType)
    {
        switch (weaponType)
        {
            case WEAPON_TYPE.HALBERD:
                halberdSlotItem.UpgradeItem();
                break;

            case WEAPON_TYPE.SWORD_SHIELD:
                swordShieldSlotItem.UpgradeItem();
                break;
        }
        OnChangeInventoryData?.Invoke(this);
    }
    public void UpgradeArmor(string armorID)
    {
        armorSlotItem.UpgradeItem();
        OnChangeInventoryData?.Invoke(this);
    }
    #endregion

    #region Response Stone Functions
    public bool TryUseResponseStone(int amount)
    {
        return responseStone >= amount;
    }

    public void BuyItem<T>(T item) where T : BaseItem
    {
        if (item is IShopableItem shopableItem)
        {
            if (TryUseResponseStone(shopableItem.ItemPrice))
            {
                if (AddItemToInventory(item))
                {
                    responseStone -= shopableItem.ItemPrice;
                    return;
                }
            }
        }
#if UNITY_EDITOR
        Debug.Log($"[Warning]: Can't buy {item.GetItemName()}");
#endif
    }
    public void RewardResponseStone(int amount)
    {
        if (amount == 0)
            return;

        responseStone += amount;
        OnChangeInventoryData?.Invoke(this);
        OnRewardResponseStone?.Invoke(amount);
    }
    #endregion

    #region Inventory Search
    public T FindInventoryItem<T>(string itemID) where T : BaseItem
    {
        for (int i = 0; i < inventoryItems.Length; ++i)
        {
            if (inventoryItems[i]?.GetItemID() == itemID)
                return inventoryItems[i] as T;
        }
#if UNITY_EDITOR
        Debug.Log($"[Warning]: There is no {Managers.DataManager.ItemTable[itemID].itemName} in inventory.");
#endif
        return null;
    }
    public int FindInventorySlotIndex(string itemID)
    {
        for (int i = 0; i < inventoryItems.Length; ++i)
        {
            if (inventoryItems[i]?.GetItemID() == itemID)
                return i;
        }
#if UNITY_EDITOR
        Debug.Log($"[Warning]: There is no {Managers.DataManager.ItemTable[itemID].itemName} in inventory.");
#endif
        return Constants.NULL_INDEX;
    }
    public int FindInventoryEmptySlotIndex()
    {
        for (int i = 0; i < inventoryItems.Length; ++i)
        {
            if (inventoryItems[i] == null)
                return i;
        }
        return Constants.NULL_INDEX;
    }
    #endregion

    #region Inventory Slot
    public void RewardItem<T>(T item) where T : BaseItem
    {
        if (item == null)
            return;

        AddItemToInventory(item);
        OnRewardItem?.Invoke(item);
    }

    public void DestroyItemInInventory(int slotIndex)
    {
        inventoryItems[slotIndex] = null;
        OnChangeInventoryData?.Invoke(this);
    }
    public bool AddItemToInventory(BaseItem item, int amount = 1)
    {
        if (item == null)
        {
#if UNITY_EDITOR
            Debug.Log("[Warning]: Can't add null");
#endif
            return false;
        }

        if (item is IStackableItem)
        {
            for (int i = 0; i < inventoryItems.Length; ++i)
            {
                if (inventoryItems[i] is IStackableItem inventoryStackableItem && inventoryItems[i].GetItemID() == item.GetItemID())
                {
                    inventoryStackableItem.ItemCount += amount;
                    OnChangeInventoryData?.Invoke(this);
                    return true;
                }
            }
        }

        int emptySlotIndex = FindInventoryEmptySlotIndex();
        if (emptySlotIndex != Constants.NULL_INDEX)
        {
            inventoryItems[emptySlotIndex] = item;
            OnChangeInventoryData?.Invoke(this);
            return true;
        }
#if UNITY_EDITOR
        Debug.Log($"[Warning]: Can't Add {item.GetItemName()}, inventory is full");
#endif
        return false;
    }
    public void InventoryToInventory(InventorySlot fromSlot, InventorySlot toSlot)
    {
        // Try Combine
        if (inventoryItems[fromSlot.SlotIndex] is IStackableItem startCountItem
            && inventoryItems[toSlot.SlotIndex] is IStackableItem endCountItem)
        {
            if (inventoryItems[fromSlot.SlotIndex].GetItemID() == inventoryItems[toSlot.SlotIndex].GetItemID())
            {
                endCountItem.ItemCount += startCountItem.ItemCount;
                inventoryItems[fromSlot.SlotIndex] = null;
                OnChangeInventoryData?.Invoke(this);
                return;
            }
        }

        // Swap
        (inventoryItems[fromSlot.SlotIndex], inventoryItems[toSlot.SlotIndex]) = (inventoryItems[toSlot.SlotIndex], inventoryItems[fromSlot.SlotIndex]);
        OnChangeInventoryData?.Invoke(this);
    }
    public void ConsumeInventoryItem(string itemID, int amount = 1)
    {
        for(int i=0; i<inventoryItems.Length; ++i)
        {
            if (inventoryItems[i].GetItemID() == itemID && inventoryItems[i] is IConsumableItem usableItem)
            {
                usableItem.Consume(Managers.DataManager.CurrentCharacterData.StatusData);
                if(usableItem is IStackableItem stackableItem)
                {
                    stackableItem.ItemCount -= amount;

                    if (stackableItem.ItemCount <= 0)
                        DestroyItemInInventory(i);
                }

                OnChangeInventoryData?.Invoke(this);
                return;
            }
        }
    }
    #endregion

    #region Quick Slot
    public void FromInventoryToQuickSlot(InventorySlot fromSlot, QuickSlot toSlot)
    {
        quickSlotItemIDs[toSlot.SlotIndex] = inventoryItems[fromSlot.SlotIndex].GetItemID();
        OnChangeInventoryData?.Invoke(this);
    }
    public void FromQuickSlotToQuickSlot(QuickSlot fromQuickSlot, QuickSlot toQuickSlot)
    {
        // Swap
        (quickSlotItemIDs[fromQuickSlot.SlotIndex], quickSlotItemIDs[toQuickSlot.SlotIndex]) = (quickSlotItemIDs[toQuickSlot.SlotIndex], quickSlotItemIDs[fromQuickSlot.SlotIndex]);
        OnChangeInventoryData?.Invoke(this);
    }
    public void ReleaseQuickSlot(QuickSlot quickSlot)
    {
        quickSlotItemIDs[quickSlot.SlotIndex] = null;
        OnChangeInventoryData?.Invoke(this);
    }
    public void UseQuickSlot(QuickSlot quickSlot)
    {
        if (!quickSlotItemIDs[quickSlot.SlotIndex].IsNullOrEmpty())
        {
            ConsumeInventoryItem(quickSlotItemIDs[quickSlot.SlotIndex]);
        }
    }
    #endregion

    #region Rune Slot
    public void FromInventoryToRuneSlot(InventorySlot fromSlot, RuneSlot toSlot)
    {
        // Swap
        if (inventoryItems[fromSlot.SlotIndex] is RuneItem)
        {
            OnUnequipRune?.Invoke(runeSlotItems[toSlot.SlotIndex]);
            OnEquipRune?.Invoke(inventoryItems[fromSlot.SlotIndex] as RuneItem);

            (runeSlotItems[toSlot.SlotIndex], inventoryItems[fromSlot.SlotIndex]) = (inventoryItems[fromSlot.SlotIndex] as RuneItem, runeSlotItems[toSlot.SlotIndex] as RuneItem);
            OnChangeInventoryData?.Invoke(this);
        }
    }
    public void FromRuneSlotToInventory(RuneSlot fromSlot, InventorySlot toSlot)
    {
        if (inventoryItems[toSlot.SlotIndex] != null && inventoryItems[toSlot.SlotIndex] is not RuneItem)
            return;

        // Swap
        OnUnequipRune?.Invoke(runeSlotItems[fromSlot.SlotIndex]);
        OnEquipRune?.Invoke(inventoryItems[toSlot.SlotIndex] as RuneItem);

        (runeSlotItems[fromSlot.SlotIndex], inventoryItems[toSlot.SlotIndex]) = (inventoryItems[toSlot.SlotIndex] as RuneItem, runeSlotItems[fromSlot.SlotIndex] as RuneItem);
        OnChangeInventoryData?.Invoke(this);
    }
    public void FromRuneSlotToRuneSlot(RuneSlot fromSlot, RuneSlot toSlot)
    {
        // Swap
        (runeSlotItems[fromSlot.SlotIndex], runeSlotItems[toSlot.SlotIndex]) = (runeSlotItems[toSlot.SlotIndex] as RuneItem, runeSlotItems[fromSlot.SlotIndex] as RuneItem);
        OnChangeInventoryData?.Invoke(this);
    }
    public void ReleaseRuneSlot(RuneSlot runeSlot)
    {
        if (AddItemToInventory(runeSlotItems[runeSlot.SlotIndex]))
        {
            OnUnequipRune?.Invoke(runeSlotItems[runeSlot.SlotIndex]);
            runeSlotItems[runeSlot.SlotIndex] = null;
            OnChangeInventoryData?.Invoke(this);
        }
    }
    #endregion

    #region Property
    [JsonIgnore] public BaseItem[] InventoryItems { get { return inventoryItems; } }
    [JsonIgnore] public RuneItem[] RuneSlotItems { get { return runeSlotItems; } }
    [JsonIgnore] public HalberdItem HalberdSlotItem { get { return halberdSlotItem; } }
    [JsonIgnore] public SwordShieldItem SwordShieldSlotItem { get { return swordShieldSlotItem; } }
    [JsonIgnore] public ArmorItem ArmorSlotItem { get { return armorSlotItem; } }
    [JsonIgnore] public ResponseWaterItem ResponseWaterSlotItem { get { return responseWaterSlotItem; } }
    [JsonIgnore] public string[] QuickSlotItemIDs { get { return quickSlotItemIDs; } set { quickSlotItemIDs = value; OnChangeInventoryData?.Invoke(this); } }
    public int ResponseStone
    {
        get { return responseStone; }
        set
        {
            responseStone = value;
            if (responseStone < 0)
                responseStone = 0;
            OnChangeInventoryData?.Invoke(this);
        }
    }
    public int ResponseWaterRemainingCount { get { return responseWaterRemainingCount; } }
    public WEAPON_TYPE CurrentWeaponType { get { return currentWeaponType; } }
    #endregion
}
