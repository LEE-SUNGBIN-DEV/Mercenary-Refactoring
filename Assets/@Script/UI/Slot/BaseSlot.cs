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
        ItemCountText,
        ItemGradeText
    }

    [Header("Base Slot")]
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemCountText;
    [SerializeField] protected TextMeshProUGUI itemGradeText;
    [SerializeField] protected int itemCount;
    [SerializeField] protected int itemGrade;
    [SerializeField] protected int slotIndex;

    public virtual void Initialize()
    {
        BindImage(typeof(IMAGE));
        BindText(typeof(TEXT));

        itemCount = 0;
        itemImage = GetImage((int)IMAGE.ItemImage);
        itemCountText = GetText((int)TEXT.ItemCountText);
        itemGradeText = GetText((int)TEXT.ItemGradeText);
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

    public void EnableGradeText(bool isEnable)
    {
        if (isEnable)
        {
            itemGradeText.text = $"+{itemGrade}";
            itemGradeText.enabled = true;
        }
        else
        {
            itemGradeText.enabled = false;
        }
    }
    public virtual void ClearSlot()
    {
        itemImage.sprite = null;
        itemImage.color = Functions.SetColor(Color.white, 0f);
        itemCount = 0;
        itemCountText.text = null;
        itemCountText.enabled = false;
        itemGrade = 0;
        itemGradeText.text = null;
        itemGradeText.enabled = false;
    }

    #region Mouse Event Function
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
    }
    public void OnEndDrag(PointerEventData eventData)
    {
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
