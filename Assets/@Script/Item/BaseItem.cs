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

[System.Serializable]
public abstract class BaseItem
{
    [Header("Base Item")]
    [SerializeField] protected int itemID;
    [SerializeField] protected ITEM_RANK itemRank;
    [SerializeField] protected string itemName;
    [SerializeField] protected string itemDescription;
    [SerializeField] protected Sprite itemSprite;

    public virtual void Initialize(ItemData itemData)
    {
        BaseItem itemInformation = Managers.DataManager.ItemTable[itemData.itemID];
        itemID = itemInformation.itemID;
        itemRank = itemInformation.itemRank;
        itemName = itemInformation.itemName;
        itemDescription = itemInformation.itemDescription;
        itemSprite = itemInformation.itemSprite;
    }
    public virtual void Initialize<T>(T item) where T:BaseItem
    {
        itemID = item.itemID;
        itemRank = item.itemRank;
        itemName = item.itemName;
        itemDescription = item.itemDescription;
        itemSprite = item.itemSprite;
    }

    #region Property
    public int ItemID { get { return itemID; } set { itemID = value; } }
    public ITEM_RANK ItemRank { get { return itemRank; } set { itemRank = value; } }
    public string ItemName { get { return itemName; } set { itemName = value; } }
    public string ItemDescription { get { return itemDescription; } set { itemDescription = value; } }
    public Sprite ItemSprite { get { return itemSprite; } set { itemSprite = value; } }
    public StatusData StatusData { get { return Managers.DataManager?.SelectCharacterData?.StatusData; } }
    #endregion
}
