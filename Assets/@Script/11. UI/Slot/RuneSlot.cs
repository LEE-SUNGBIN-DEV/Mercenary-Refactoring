using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class RuneSlot : BaseItemSlot, ITooltipItemSlot, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItemTooltipPanel tooltipPanel;

    public ItemTooltipPanel TooltipPanel { get { return tooltipPanel; } set { tooltipPanel = value; } }

    public override void UpdateSlot(CharacterInventoryData inventoryData)
    {
        base.UpdateSlot(inventoryData);
        RuneItem item = inventoryData.RuneSlotItems[slotIndex];
        if (item != null)
        {
            itemImage.sprite = item.GetItemSprite();
            itemImage.color = new Color32(255, 255, 255, 255);
            HideAmountText();
            HideGradeText();
        }
    }

    public override void SlotEndDrag()
    {
        if (ToSlot is InventorySlot toInventorySlot)
        {
            inventoryData?.FromRuneSlotToInventory(this, toInventorySlot);
        }
        if (ToSlot is RuneSlot toRuneSlot)
        {
            inventoryData?.FromRuneSlotToRuneSlot(this, toRuneSlot);
        }
    }
    public override void OnSlotRightClicked(PointerEventData eventData)
    {
        inventoryData?.ReleaseRuneSlot(this);
    }

    public void ShowTooltip()
    {
        tooltipPanel.ShowTooltip(inventoryData.RuneSlotItems[slotIndex], inventoryData);
    }
    public void HideTooltip()
    {
        tooltipPanel.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowHighlight();
        ShowTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideHighlight();
        HideTooltip();
    }
}
