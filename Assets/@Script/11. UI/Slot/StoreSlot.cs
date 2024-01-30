using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public class StoreSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private BaseItem sellItem;
    [SerializeField] private Image storeSlotImage;
    [SerializeField] private TextMeshProUGUI storeSlotItemNameText;
    [SerializeField] private TextMeshProUGUI storeSlotItemPriceText;

    public void Initialize(BaseItem sellItem)
    {
        if (sellItem is IShopableItem shopableItem)
        {
            this.sellItem = sellItem;
            storeSlotImage.sprite = SellItem.GetItemSprite();
            storeSlotItemNameText.text = SellItem.GetItemName();
            storeSlotItemPriceText.text = shopableItem.ItemPrice.ToString() + "G";
            storeSlotImage.color = new Color32(255, 255, 255, 255);
        }
    }
    public void BuyItem()
    {
        Managers.DataManager.CurrentCharacterData.InventoryData.BuyItem(sellItem);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && SellItem != null)
        {
            BuyItem();
        }
    }

    #region Property
    public BaseItem SellItem { get { return sellItem; } }
    public Image StoreSlotImage { get { return storeSlotImage; } }
    #endregion
}
