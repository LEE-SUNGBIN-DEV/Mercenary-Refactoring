using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ConsumptionItem : BaseItem
{
    public ConsumptionItem()
    {

    }
    public ConsumptionItem(BaseItem baseItem)
    {
        itemType = baseItem.ItemType;
        itemRank = baseItem.ItemRank;
        itemID = baseItem.ItemID;
        itemName = baseItem.ItemName;
        itemDescription = baseItem.ItemDescription;
        itemCount = baseItem.ItemCount;
        itemPrice = baseItem.ItemPrice;
        isCountable = baseItem.IsCountable;
        itemSprite = baseItem.ItemSprite;
    }

    public virtual void Consume(Character character) {; }
}
