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
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemCountText;
    [SerializeField] protected int itemCount;
    [SerializeField] protected int slotIndex;

    public virtual void Initialize()
    {
        BindImage(typeof(IMAGE));
        BindText(typeof(TEXT));

        itemCount = 0;
        itemImage = GetImage((int)IMAGE.ItemImage);
        itemCountText = GetText((int)TEXT.ItemCountText);
    }

    public void EnableCountText(bool isEnable)
    {
        if(isEnable)
        {
            itemCountText.text = $"{itemCount}";
            itemCountText.enabled = true;
        }
        else
        {
            itemCountText.enabled = false;
        }
    }

    public virtual void SetSlotByItem<T>(T requestItem) where T : BaseItem
    {
        if(requestItem != null)
        {
            itemImage.sprite = requestItem.ItemSprite;
            itemImage.color = Functions.SetColor(Color.white, 1f);
            EnableCountText(requestItem is CountItem);
        }
    }
    public virtual void LoadSlot(ItemData itemSaveData)
    {
        if (itemSaveData != null)
        {
            BaseItem item = Managers.DataManager.ItemTable[itemSaveData.itemID];
            SetSlotByItem(item);
        }
    }
    public void ClearSlot()
    {
        itemImage.sprite = null;
        itemImage.color = Functions.SetColor(Color.white, 0f);
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
            Debug.Log("Right Click");
            SlotRightClick(eventData);
        }
    }
    #endregion

    #region Property
    public Image ItemImage { get { return itemImage; } set { itemImage = value; } }
    public TextMeshProUGUI ItemCountText { get { return itemCountText; } set { itemCountText = value; } }
    public int ItemCount { get { return itemCount; } set { itemCount = value; } }
    public int SlotIndex { get { return slotIndex; } set { slotIndex = value; } }
    #endregion
}
