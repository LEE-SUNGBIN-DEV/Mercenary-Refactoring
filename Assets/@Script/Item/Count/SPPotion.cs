using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SPPotion : CountItem, IUsableItem, IShopableItem
{
    [Header("SP Potion")]
    protected float recoveryAmount;
    protected int itemPrice;

    public override void Initialize<T>(T item)
    {
        base.Initialize(item);
        if (item is SPPotion targetItem)
        {
            recoveryAmount = targetItem.recoveryAmount;
            itemPrice = targetItem.itemPrice;
        }
    }

    public void UseItem(Character character)
    {
        Managers.AudioManager.PlaySFX("Potion Consume");
        character.Status.CurrentStamina += recoveryAmount;
    }

    public void BuyItem(Character character)
    {
    }

    public void SellItem(Character character)
    {
    }

    public float RecoveryAmount { get { return recoveryAmount; } }
    public int ItemPrice { get { return itemPrice; } set { itemPrice = value; } }
}
