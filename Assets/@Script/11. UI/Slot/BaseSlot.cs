using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public abstract class BaseSlot : UIBase, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum IMAGE
    {
        Item_Image,
        Highlight_Image
    }
    public enum TEXT
    {
        Item_Count_Text,
        Item_Grade_Text
    }

    [Header("Base Slot")]
    [SerializeField] protected Image highlightImage;
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

        itemImage = GetImage((int)IMAGE.Item_Image);
        highlightImage = GetImage((int)IMAGE.Highlight_Image);
        itemCountText = GetText((int)TEXT.Item_Count_Text);
        itemGradeText = GetText((int)TEXT.Item_Grade_Text);
        itemCount = 0;
        EnableHighlight(false);
    }

    public void EnableHighlight(bool isEnable)
    {
        if (isEnable)
            highlightImage.enabled = true;
        else
            highlightImage.enabled = false;
    }
    public void EnableCountText(bool isEnable)
    {
        if (isEnable)
        {
            itemCountText.text = $"{itemCount}";
            itemCountText.enabled = true;
        }
        else
            itemCountText.enabled = false;
    }

    public void EnableGradeText(bool isEnable)
    {
        if (isEnable)
        {
            itemGradeText.text = $"+{itemGrade}";
            itemGradeText.enabled = true;
        }
        else
            itemGradeText.enabled = false;
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
    public abstract void EndDrag();
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        EnableHighlight(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        EnableHighlight(false);
    }
    #endregion

    #region Property
    public Image HighlightImage { get { return highlightImage; } set { highlightImage = value; } }
    public Image ItemImage { get { return itemImage; } set { itemImage = value; } }
    public TextMeshProUGUI ItemCountText { get { return itemCountText; } set { itemCountText = value; } }
    public int ItemCount { get { return itemCount; } set { itemCount = value; } }
    public int SlotIndex { get { return slotIndex; } set { slotIndex = value; } }
    public BaseSlot EndSlot { get { return Managers.SlotManager?.EndSlot; } }
    public CharacterInventoryData InventoryData { get { return Managers.DataManager?.CurrentCharacterData?.InventoryData; } }
    #endregion
}
