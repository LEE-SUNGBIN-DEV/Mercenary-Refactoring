using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopableItem
{
    public int ItemPrice { get; set; }

    public void BuyItem(BaseCharacter character);
    public void SellItem(BaseCharacter character);
}
