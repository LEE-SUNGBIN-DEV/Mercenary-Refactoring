using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEM_RANK
{
    Rare,
    Epic,
    Regendary,
    Myth
}

public enum ITEM_TYPE
{
    Normal,
    Equipment,
    Consumption,
    Quest
}

[System.Serializable]
public class BaseItem
{
    [Header("Base Item")]
    protected ITEM_TYPE itemType;
    protected ITEM_RANK itemRank;
    protected int itemID;
    protected string itemName;
    protected string itemDescription;
    protected int itemCount;
    protected int itemPrice;
    protected bool isCountable;
    protected Sprite itemSprite;

    #region Property
    public ITEM_TYPE ItemType { get { return itemType; } set { itemType = value; } }
    public ITEM_RANK ItemRank { get { return itemRank; } set { itemRank = value; } }
    public int ItemID { get { return itemID; } set { itemID = value; } }
    public string ItemName { get { return itemName; } set { itemName = value; } }
    public string ItemDescription { get { return itemDescription; } set { itemDescription = value; } }
    public int ItemCount { get { return itemCount; } set { itemCount = value; } }
    public int ItemPrice { get { return itemPrice; } set { itemPrice = value; } }
    public bool IsCountable { get { return isCountable; } set { isCountable = value; } }
    public Sprite ItemSprite { get { return itemSprite; } set { itemSprite = value; } }
    #endregion
}
