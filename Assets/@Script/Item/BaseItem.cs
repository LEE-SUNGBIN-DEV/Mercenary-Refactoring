using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseItem : MonoBehaviour
{
    [Header("Base Item")]
    protected int itemID;
    protected string itemName;
    protected string itemDescription;
    protected int itemPrice;
    protected bool isCountable;
    protected Sprite itemSprite;

    #region Property
    public int ItemID { get { return itemID; } set { itemID = value; } }
    public string ItemName { get { return itemName; } set { itemName = value; } }
    public string ItemDescription { get { return itemDescription; } set { itemDescription = value; } }
    public int ItemPrice { get { return itemPrice; } set { itemPrice = value; } }
    public bool IsCountable { get { return isCountable; } set { isCountable = value; } }
    public Sprite ItemSprite { get { return itemSprite; } set { itemSprite = value; } }
    #endregion
}
