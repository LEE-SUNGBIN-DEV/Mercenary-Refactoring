using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SLOT_TYPE
{
    Inventory = 1,
    QuickSlot = 2,

    WeaponSlot = 100,
    HelmetSlot = 101,
    ArmorSlot = 102,
    BootsSlot = 103,
}

[System.Serializable]
public class ItemSaveData
{
    [Header("Item Location")]
    private SLOT_TYPE slotType;
    private int slotIndex;

    [Header("Base Item")]
    private int itemID;
    private int itemCount;

    [Header("Equipment Item")]
    private int grade;

    #region Property
    public SLOT_TYPE SlotType { get { return slotType; } set { slotType = value; } }
    public int SlotIndex { get { return slotIndex; } set { slotIndex = value; } }
    public int ItemID { get { return itemID; } set { itemID = value; } }
    public int ItemCount { get { return itemCount; } set { itemCount = value; } }
    public int Grade { get { return grade; } set { grade = value; } }
    #endregion
}
