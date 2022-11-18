using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EquipmentItem : BaseItem
{
    [Header("Equipment Item")]
    protected int grade;
    
    public EquipmentItem()
    {

    }
    public EquipmentItem(BaseItem baseItem)
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
        grade = 0;
    }

    public virtual void Equip(Character character)
    {
        Managers.AudioManager.PlaySFX("Audio_Equipment_Mount");
    }
    public virtual void Release(Character character)
    {
        Managers.AudioManager.PlaySFX("Audio_Equipment_Dismount");
    }

    public int Grade { get { return grade; } set { grade = value; } }
}
