using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ======================================
//              Legacy Script
// ======================================

/*
public enum DRAG_TYPE
{
    NONE,
    EXCHANGE,
    MOVE,
    COMBINE,
    CANCEL
}
public class DragSlot : MonoBehaviour
{
    private static DragSlot instance = null;

    [SerializeField] private DRAG_TYPE dragType;
    [SerializeField] private Legacy_Slot selectSlot;
    [SerializeField] private Image slotImage;

    [SerializeField] private BaseItem item;
    [SerializeField] private int itemCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            ClearDragSlot();
        }

        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ExchangeItem(BaseItem temporaryItem, int temporaryItemCount)
    {
        Item = temporaryItem;
        ItemCount = temporaryItemCount;
        SelectSlot.SetEndDragStrategy(new SlotEndDragExChange());
    }

    public void CombineItem()
    {
        SelectSlot.SetEndDragStrategy(new SlotEndDragCombine());
    }

    public void CancelItem()
    {
        Managers.UIManager.RequestNotice("잘못된 입력입니다.");
        SelectSlot.SetEndDragStrategy(new SlotEndDragCancel());
    }

    public bool IsEmpty()
    {
        if (Item == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ClearDragSlot()
    {
        DragType = DRAG_TYPE.NONE;
        SelectSlot = null;
        SlotImage.sprite = null;
        Item = null;
        ItemCount = 0;
        EnableSlotImage(false);
    }

    public void SetDragSlot(Legacy_Slot currentSlot)
    {
        DragType = DRAG_TYPE.NONE;
        SelectSlot = currentSlot;
        SlotImage.sprite = currentSlot.Item.ItemSprite;
        Item = currentSlot.Item;
        ItemCount = currentSlot.ItemCount;
        EnableSlotImage(true);
    }

    public void EnableSlotImage(bool isEnable)
    {
        SetImageAlpha(0.7f);
        SlotImage.enabled = isEnable;
    }

    public void SetImageAlpha(float alphaValue)
    {
        Color alpha = SlotImage.color;
        alpha.a = alphaValue;
        SlotImage.color = alpha;
    }

    #region Property
    public static DragSlot Instance
    {
        get { return instance; }
        private set { instance = value; }
    }
    public DRAG_TYPE DragType
    {
        get { return dragType; }
        set { dragType = value; }
    }
    public Legacy_Slot SelectSlot
    {
        get { return selectSlot; }
        set { selectSlot = value; }
    }
    public Image SlotImage
    {
        get { return slotImage; }
        set { slotImage = value; }
    }
    public BaseItem Item
    {
        get { return item; }
        set { item = value; }
    }
    public int ItemCount
    {
        get { return itemCount; }
        set { itemCount = value; }
    }
    #endregion
}
*/