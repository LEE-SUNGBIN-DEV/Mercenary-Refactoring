using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemData
{
    [Header("Base Item")]
    public int itemID;

    [Header("Count Item")]
    public int itemCount;

    [Header("Equipment Item")]
    public int grade;

    public ItemData()
    {
    }
    public ItemData(BaseItem item)
    {
        itemID = item.ItemID;
        itemCount = 1;
        grade = 0;
}
    public ItemData(CountItem item)
    {
        itemID = item.ItemID;
        itemCount = item.ItemCount;
        grade = 0;
    }
    public ItemData(EquipmentItem item)
    {
        itemID = item.ItemID;
        itemCount = 1;
        grade = item.Grade;
    }
}
