using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopableItem
{
    public int ItemPrice { get; set; }

    public void BuyItem(PlayerCharacter character);
    public void SellItem(PlayerCharacter character);
}
