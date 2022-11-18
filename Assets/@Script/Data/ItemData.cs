using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    [Header("Item Location")]
    public int slotIndex;

    [Header("Base Item")]
    public ITEM_TYPE itemType;
    public int itemID;
    public int itemCount;

    [Header("Equipment Item")]
    public int grade;

    public ItemData()
    {
    }
    public ItemData(BaseItem item, int index)
    {
        slotIndex = index;
        itemType = item.ItemType;
        itemID = item.ItemID;
        itemCount = item.ItemCount;
        grade = 0;
    }
    public ItemData(EquipmentItem item, int index)
    {
        slotIndex = index;
        itemType = item.ItemType;
        itemID = item.ItemID;
        itemCount = item.ItemCount;
        grade = item.Grade;
    }
}
