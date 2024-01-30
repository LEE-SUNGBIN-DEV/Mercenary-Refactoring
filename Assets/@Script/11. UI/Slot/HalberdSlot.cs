using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class HalberdSlot : BaseItemSlot, ITooltipItemSlot, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItemTooltipPanel tooltipPanel;

    public ItemTooltipPanel TooltipPanel { get { return tooltipPanel; } set { tooltipPanel = value; } }

    public override void UpdateSlot(CharacterInventoryData inventoryData)
    {
        base.UpdateSlot(inventoryData);
        HalberdItem item = inventoryData.HalberdSlotItem;
        if (item != null)
        {
            itemImage.sprite = item.GetItemSprite();
            itemImage.color = new Color32(255, 255, 255, 255);
            HideAmountText();
            HideGradeText();
        }
    }

    public void ShowTooltip()
    {
        tooltipPanel.ShowTooltip(inventoryData.HalberdSlotItem, inventoryData);
    }
    public void HideTooltip()
    {
        tooltipPanel.HideTooltip();
    }
    public override void SlotEndDrag()
    {
    }
    public override void OnSlotRightClicked(PointerEventData eventData)
    {
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
