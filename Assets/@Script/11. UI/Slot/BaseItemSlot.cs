using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[System.Serializable]
public abstract class BaseItemSlot : UIBase, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
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
    [SerializeField] protected RectTransform rectTransform;
    [SerializeField] protected Image highlightImage;
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemCountText;
    [SerializeField] protected TextMeshProUGUI itemGradeText;
    [SerializeField] protected int itemCount;
    [SerializeField] protected int itemGrade;
    [SerializeField] protected int slotIndex;

    protected CharacterInventoryData inventoryData;

    #region Private
    private void OnDisable()
    {
        if (inventoryData != null)
        {
            inventoryData = null;
        }
    }
    #endregion

    public virtual void Initialize(int slotIndex = 0)
    {
        this.slotIndex = slotIndex;

        TryGetComponent(out rectTransform);

        BindImage(typeof(IMAGE));
        BindText(typeof(TEXT));

        highlightImage = GetImage((int)IMAGE.Highlight_Image);
        itemImage = GetImage((int)IMAGE.Item_Image);
        itemCountText = GetText((int)TEXT.Item_Count_Text);
        itemGradeText = GetText((int)TEXT.Item_Grade_Text);
        itemCount = 0;

        ClearSlot();
    }
    public virtual void UpdateSlot(CharacterInventoryData inventoryData)
    {
        ClearSlot();
        this.inventoryData = inventoryData;
    }
    public virtual void ClearSlot()
    {
        itemImage.sprite = null;
        itemImage.color = new Color32(255, 255, 255, 0);
        itemCount = 0;
        itemCountText.text = null;
        itemCountText.enabled = false;
        itemGrade = 0;
        itemGradeText.text = null;
        itemGradeText.enabled = false;

        HideAmountText();
        HideGradeText();
        HideHighlight();
    }

    public abstract void OnSlotRightClicked(PointerEventData eventData);
    public abstract void SlotEndDrag();

    public virtual void ShowGradeText()
    {
        itemGradeText.text = $"+{itemGrade}";
        itemGradeText.enabled = true;
    }
    public virtual void HideGradeText()
    {
        itemGradeText.gameObject.SetActive(false);
    }
    public virtual void ShowAmountText()
    {
        itemCountText.text = $"{itemCount}";
        itemCountText.enabled = true;
    }
    public virtual void HideAmountText()
    {
        itemCountText.gameObject.SetActive(false);
    }
    public virtual void ShowHighlight()
    {
        highlightImage.gameObject.SetActive(true);
    }
    public virtual void HideHighlight()
    {
        highlightImage.gameObject.SetActive(false);
    }

    #region Unity Interface Functions
    public void OnBeginDrag(PointerEventData eventData)
    {
        Managers.UIManager.UISystemPanelCanvas.DragSlotPanel.BeginDrag(this);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Managers.UIManager.UISystemPanelCanvas.DragSlotPanel.Drag(eventData);
    }
    public void OnDrop(PointerEventData eventData)
    {
        Managers.UIManager.UISystemPanelCanvas.DragSlotPanel.Drop(this);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        SlotEndDrag();
        Managers.UIManager.UISystemPanelCanvas.DragSlotPanel.EndDrag(this);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right Click");
            OnSlotRightClicked(eventData);
        }
    }
    #endregion

    #region Property
    public Image ItemImage { get { return itemImage; } }
    public int SlotIndex { get { return slotIndex; } set { slotIndex = value; } }
    public BaseItemSlot ToSlot { get { return Managers.UIManager.UISystemPanelCanvas.DragSlotPanel.ToSlot; } }
    #endregion
}
