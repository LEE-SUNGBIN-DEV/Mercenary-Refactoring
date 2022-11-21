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

    Weapon,
    Helmet,
    Armor,
    Boots,

    HPPotion,
    SPPotion,

    Quest
}

[System.Serializable]
public abstract class BaseItem
{
    [Header("Base Item")]
    [SerializeField] protected ITEM_TYPE itemType;
    [SerializeField] protected ITEM_RANK itemRank;
    [SerializeField] protected int itemID;
    [SerializeField] protected string itemName;
    [SerializeField] protected string itemDescription;
    [SerializeField] protected Sprite itemSprite;

    public virtual void Initialize<T>(T item) where T:BaseItem
    {
        itemType = item.itemType;
        itemRank = item.itemRank;
        itemID = item.itemID;
        itemName = item.itemName;
        itemDescription = item.itemDescription;
        itemSprite = item.itemSprite;
    }

    #region Property
    public ITEM_TYPE ItemType { get { return itemType; } set { itemType = value; } }
    public ITEM_RANK ItemRank { get { return itemRank; } set { itemRank = value; } }
    public int ItemID { get { return itemID; } set { itemID = value; } }
    public string ItemName { get { return itemName; } set { itemName = value; } }
    public string ItemDescription { get { return itemDescription; } set { itemDescription = value; } }
    public Sprite ItemSprite { get { return itemSprite; } set { itemSprite = value; } }
    #endregion
}
