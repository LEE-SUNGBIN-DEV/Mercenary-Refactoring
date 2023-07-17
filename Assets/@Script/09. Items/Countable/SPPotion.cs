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

    public void UseItem(CharacterStatusData statusData)
    {
        //Managers.AudioManager.PlaySFX("Potion Consume");
        statusData.RecoverStamina(recoveryAmount, CALCULATE_MODE.Ratio);
    }

    public void BuyItem(PlayerCharacter character)
    {
    }

    public void SellItem(PlayerCharacter character)
    {
    }

    public float RecoveryAmount { get { return recoveryAmount; } }
    public int ItemPrice { get { return itemPrice; } set { itemPrice = value; } }
}
