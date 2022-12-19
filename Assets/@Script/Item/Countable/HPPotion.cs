using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HPPotion : CountItem, IUsableItem, IShopableItem
{
    [Header("HP Potion")]
    protected float recoveryAmount;
    protected int itemPrice;

    public override void Initialize<T>(T item)
    {
        base.Initialize(item);
        if (item is HPPotion targetItem)
        {
            recoveryAmount = targetItem.recoveryAmount;
            itemPrice = targetItem.itemPrice;
        }
    }

    public void UseItem(StatusData statusData)
    {
        Debug.Log("Use Item");
        //Managers.AudioManager.PlaySFX("Potion Consume");
        statusData.CurrentHP += recoveryAmount;
    }

    public void BuyItem(BaseCharacter character)
    {
    }

    public void SellItem(BaseCharacter character)
    {
    }

    public float RecoveryAmount {  get { return recoveryAmount; } }
    public int ItemPrice { get { return itemPrice; } set { itemPrice = value; } }
}
