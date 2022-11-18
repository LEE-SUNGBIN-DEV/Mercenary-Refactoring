using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public abstract class BaseSlot: UIBase, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public enum IMAGE
    {
        ItemImage
    }
    public enum TEXT
    {
        ItemCountText
    }

    [Header("Base Slot")]
    [SerializeField] protected BaseItem item;
    [SerializeField] protected int itemCount;
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemCountText;

    public virtual void Initialize()
    {
        BindImage(typeof(IMAGE));
        BindText(typeof(TEXT));

        itemImage = GetImage((int)IMAGE.ItemImage);
        itemCountText = GetText((int)TEXT.ItemCountText);

        item = null;
        itemCount = 0;
    }

    public bool IsEmpty()
    {
        return item == null;
    }

    public void EnableOrDisableCountText()
    {
        if (item.IsCountable)
        {
            itemCountText.text = $"{itemCount}";
            itemCountText.enabled = true;
        }
        else
        {
            itemCountText.enabled = false;
        }
    }
    public void SetItemToSlot<T>(T targetItem) where T : BaseItem
    {
        item = targetItem;
        itemImage.sprite = targetItem.ItemSprite;
        itemImage.color = Color.white;
        itemCount = targetItem.ItemCount;
        EnableOrDisableCountText();
    }
    public void AddItemToSlot<T>(T targetItem) where T : BaseItem
    {
        if(IsEmpty())
        {
            SetItemToSlot(targetItem);
        }
        else
        {
            itemCount += targetItem.ItemCount;
            EnableOrDisableCountText();
        }
    }
    public void RemoveItemFromSlot(int amount = 1)
    {
        itemCount -= amount;
        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    public void SetSlot(BaseSlot slot)
    {
        if(!slot.IsEmpty())
        {
            item = slot.Item;
            itemImage.sprite = slot.Item.ItemSprite;
            itemCount = slot.ItemCount;
            EnableOrDisableCountText();
        }
    }
    public void ClearSlot()
    {
        itemImage.color = Color.clear;
        item = null;
        itemImage.sprite = null;
        itemCount = 0;
        itemCountText.text = null;
        itemCountText.enabled = false;
    }

    #region Mouse Event Function
    public abstract void Drop();
    public abstract void EndDrag();
    public abstract void SlotRightClick(PointerEventData eventData);

    public void OnBeginDrag(PointerEventData eventData)
    {
        Managers.SlotManager.OnBeginDrag(this);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Managers.SlotManager.OnDrag(eventData);
    }
    public void OnDrop(PointerEventData eventData)
    {
        Managers.SlotManager.OnDrop(this);
        Drop();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag();
        Managers.SlotManager.OnEndDrag();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            SlotRightClick(eventData);
        }
    }
    #endregion

    #region Property
    public BaseItem Item { get { return item; } set { item = value; } }
    public int ItemCount { get { return itemCount; } set { itemCount = value; } }
    public Image ItemImage { get { return itemImage; } set { itemImage = value; } }
    public TextMeshProUGUI ItemCountText { get { return itemCountText; } set { itemCountText = value; } }
    #endregion
}
