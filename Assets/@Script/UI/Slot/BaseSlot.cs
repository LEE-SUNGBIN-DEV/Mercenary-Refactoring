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
    protected int slotIndex;

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

    public virtual void SetItemToSlot<T>(T targetItem) where T : BaseItem
    {
        item = targetItem;
        itemImage.sprite = targetItem.ItemSprite;
        itemImage.color = Color.white;
        itemCount = targetItem.ItemCount;
        EnableOrDisableCountText();
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
    public void LoadSlot(ItemSaveData itemSaveData)
    {
        if(itemSaveData != null)
        {
            switch (itemSaveData.itemType)
            {
                case ITEM_TYPE.Normal:
                    {
                        BaseItem item = Managers.DataManager.ItemTable[itemSaveData.itemID];
                        SetItemToSlot(item);

                        break;
                    }
                case ITEM_TYPE.Equipment:
                    {
                        EquipmentItem item = Managers.DataManager.ItemTable[itemSaveData.itemID] as EquipmentItem;
                        item.Grade = itemSaveData.grade;
                        SetItemToSlot(item);

                        break;
                    }
                case ITEM_TYPE.Consumption:
                    {
                        ConsumptionItem item = Managers.DataManager.ItemTable[itemSaveData.itemID] as ConsumptionItem;
                        SetItemToSlot(item);

                        break;
                    }
                case ITEM_TYPE.Quest:
                    {
                        BaseItem item = Managers.DataManager.ItemTable[itemSaveData.itemID];
                        SetItemToSlot(item);

                        break;
                    }
            }
        }
    }
    public void ClearSlot()
    {
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
        Managers.UIManager.SlotController.OnBeginDrag(this);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Managers.UIManager.SlotController.OnDrag(eventData);
    }
    public void OnDrop(PointerEventData eventData)
    {
        Managers.UIManager.SlotController.OnDrop(this);
        Drop();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag();
        Managers.UIManager.SlotController.OnEndDrag();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right Click");
            SlotRightClick(eventData);
        }
    }
    #endregion

    #region Property
    public BaseItem Item { get { return item; } set { item = value; } }
    public int ItemCount { get { return itemCount; } set { itemCount = value; } }
    public Image ItemImage { get { return itemImage; } set { itemImage = value; } }
    public TextMeshProUGUI ItemCountText { get { return itemCountText; } set { itemCountText = value; } }
    public int SlotIndex { get { return slotIndex; } set { slotIndex = value; } }
    #endregion
}
