using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseItem
{
    [Header("Base Item")]
    [SerializeField] protected ItemData itemData;
    [SerializeField] protected StatOption[] fixedOptions;
    [SerializeField] protected StatOption[] randomOptions;
    [SerializeField] protected int itemCount;

    public BaseItem(string itemID)
    {
        Managers.DataManager.ItemTable.TryGetValue(itemID, out itemData);
        itemCount = 1;
    }

    public virtual void LoadFromItemID(string itemID)
    {
        if (Managers.DataManager.ItemTable.TryGetValue(itemID, out itemData))
        {
            itemCount = 1;
            CreateFixedOptions();
        }
    }
    public virtual void LoadFromSaveData(ItemSaveData itemSaveData)
    {
        if (itemSaveData?.itemID != null && Managers.DataManager.ItemTable.TryGetValue(itemSaveData.itemID, out itemData))
        {
            itemCount = itemSaveData.itemCount;
            // Create Fixed Options
            CreateFixedOptions();
            
            // Create Random Options
            if(!itemSaveData.randomStatIDs.IsNullOrEmpty())
            {
                randomOptions = new StatOption[itemSaveData.randomStatIDs.Length];
                for (int i = 0; i < itemSaveData.randomStatIDs.Length; i++)
                {
                    randomOptions[i] = new StatOption(itemSaveData.randomStatIDs[i], itemSaveData.randomStatValueTypes[i], itemSaveData.randomStatValues[i]);
                }
            }
        }
    }
    public void UpgradeItem()
    {
        if (!IsMaxGrade())
        {
            LoadFromItemID(itemData.nextGradeID);
        }
    }
    public bool IsMaxGrade()
    {
        return itemData.nextGradeID.IsNullOrEmpty();
    }
    public void CreateFixedOptions()
    {
        if (itemData?.fixedOptionID != null && Managers.DataManager.FixedOptionTable.TryGetValue(itemData.fixedOptionID, out FixedOptionData fixedOptionData))
        {
            fixedOptions = fixedOptionData.CreateFixedOptions();
        }
    }
    public void CreateRandomOptions()
    {
        if (itemData?.randomOptionID != null && Managers.DataManager.RandomOptionTable.TryGetValue(itemData?.randomOptionID, out RandomOptionData randomOptionData))
        {
            randomOptions = randomOptionData.CreateRandomOptions();
        }
    }
    public string GetItemID()
    {
        return itemData?.itemID;
    }
    public string GetItemName()
    {
        return itemData.itemName;
    }
    public string GetItemTypeText()
    {
        switch (itemData.itemType)
        {
            case ITEM_TYPE.NORMAL:
                return Managers.DataManager.TextTable[Constants.TEXT_ITEM_TYPE_NORMAL].textContent;
            case ITEM_TYPE.UNIQUE_HALBERD:
            case ITEM_TYPE.UNIQUE_SWORD_SHIELD:
            case ITEM_TYPE.UNIQUE_ARMOR:
            case ITEM_TYPE.UNIQUE_RESPONSE_WATER:
                return Managers.DataManager.TextTable[Constants.TEXT_ITEM_TYPE_UNIQUE_EQUIPMENT].textContent;
            case ITEM_TYPE.RUNE:
                return Managers.DataManager.TextTable[Constants.TEXT_ITEM_TYPE_RUNE].textContent;

            default:
                return null;
        }
    }
    public string GetItemRankText()
    {
        switch (itemData.itemRank)
        {
            case ITEM_RANK.COMMON:
                return Managers.DataManager.TextTable[Constants.TEXT_ITEM_RANK_COMMON].textContent;
            case ITEM_RANK.RARE:
                return Managers.DataManager.TextTable[Constants.TEXT_ITEM_RANK_RARE].textContent;
            case ITEM_RANK.EPIC:
                return Managers.DataManager.TextTable[Constants.TEXT_ITEM_RANK_EPIC].textContent;
            case ITEM_RANK.REGENDARY:
                return Managers.DataManager.TextTable[Constants.TEXT_ITEM_RANK_REGENDARY].textContent;
            case ITEM_RANK.MYTH:
                return Managers.DataManager.TextTable[Constants.TEXT_ITEM_RANK_MYTH].textContent;
            case ITEM_RANK.UNIQUE:
                return Managers.DataManager.TextTable[Constants.TEXT_ITEM_RANK_UNIQUE].textContent;

            default: return null;
        }
    }
    public Color GetItemRankColor()
    {
        switch (itemData.itemRank)
        {
            case ITEM_RANK.COMMON:
                return Color.white;
            case ITEM_RANK.RARE:
                return new Color32(100, 255, 0, 255); // Green
            case ITEM_RANK.EPIC:
                return new Color32(0, 150, 255, 255); //Blue
            case ITEM_RANK.REGENDARY:
                return new Color32(255, 160, 0, 255); //Orange
            case ITEM_RANK.MYTH:
                return new Color32(255, 40, 0, 255);
            case ITEM_RANK.UNIQUE:                
                return new Color32(255, 80, 0, 255);
            default:
                return Color.white;
        }
    }
    public Sprite GetItemSprite()
    {
        return itemData?.GetItemSprite();
    }
    public virtual string GetItemDescription()
    {
        return itemData?.itemDescription;
    }
    public bool IsExistItemData()
    {
        return (itemData != null);
    }

    #region Property
    public ItemData ItemData { get { return itemData; } }
    public StatOption[] FixedOptions { get { return fixedOptions; } }
    public StatOption[] RandomOptions { get { return randomOptions; } }
    public int ItemCount { get { return itemCount; } }
    #endregion
}
