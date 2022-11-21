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
    [SerializeField] private BaseItem item;
    [SerializeField] private Image storeSlotImage;
    [SerializeField] private TextMeshProUGUI storeSlotItemNameText;
    [SerializeField] private TextMeshProUGUI storeSlotItemPriceText;

    public void Initialize(BaseItem sellItem)
    {
        if (sellItem is IShopableItem shopableItem)
        {
            item = sellItem;
            storeSlotImage.sprite = Item.ItemSprite;
            storeSlotItemNameText.text = Item.ItemName;
            storeSlotItemPriceText.text = shopableItem.ItemPrice.ToString() + "G";
            storeSlotImage.color = Functions.SetColor(StoreSlotImage.color, 1f);
        }
    }
    public void BuyItem()
    {
        Managers.DataManager.SelectCharacterData.InventoryData.BuyItem(this);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && Item != null)
        {
            BuyItem();
        }
    }

    #region Property
    public BaseItem Item { get { return item; } }
    public Image StoreSlotImage { get { return storeSlotImage; } }
    #endregion
}
