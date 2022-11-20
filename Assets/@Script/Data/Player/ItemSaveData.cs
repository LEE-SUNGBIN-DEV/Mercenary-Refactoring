using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSaveData
{
    [Header("Base Item")]
    public ITEM_TYPE itemType;
    public int itemID;
    public int itemCount;

    [Header("Equipment Item")]
    public int grade;

    public ItemSaveData()
    {
    }
    public ItemSaveData(int itemID)
    {
        this.itemID = itemID;
    }
    public ItemSaveData(BaseItem item)
    {
        itemType = item.ItemType;
        itemID = item.ItemID;
        itemCount = item.ItemCount;
        grade = 0;
    }
    public ItemSaveData(EquipmentItem item)
    { 
        itemType = item.ItemType;
        itemID = item.ItemID;
        itemCount = item.ItemCount;
        grade = item.Grade;
    }
    public ItemSaveData(ConsumptionItem item)
    {
        itemType = item.ItemType;
        itemID = item.ItemID;
        itemCount = item.ItemCount;
        grade = 0;
    }
}
