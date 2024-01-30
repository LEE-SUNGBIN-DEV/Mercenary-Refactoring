using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : BaseItemSlot, ITooltipItemSlot, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ItemTooltipPanel tooltipPanel;

    public ItemTooltipPanel TooltipPanel { get { return tooltipPanel; } set { tooltipPanel = value; } }

    public override void UpdateSlot(CharacterInventoryData inventoryData)
    {
        base.UpdateSlot(inventoryData);
        BaseItem item = inventoryData.InventoryItems[slotIndex];
        if (item != null)
        {
            itemImage.sprite = item.GetItemSprite();
            itemImage.color = new Color32(255, 255, 255, 255);

            HideGradeText();
            HideAmountText();

            if (item is IStackableItem stackableItem)
            {
                itemCount = stackableItem.ItemCount;
                ShowAmountText();
            }
        }
    }

    #region Mouse Event
    public override void SlotEndDrag()
    {
        if (ToSlot is InventorySlot toInventorySlot)
            inventoryData?.InventoryToInventory(this, toInventorySlot);

        else if (ToSlot is QuickSlot toQuickSlot)
            inventoryData?.FromInventoryToQuickSlot(this, toQuickSlot);

        else if (ToSlot is RuneSlot toRuneSlot)
            inventoryData?.FromInventoryToRuneSlot(this, toRuneSlot);
    }

    public override void OnSlotRightClicked(PointerEventData eventData)
    {
    }

    public void ShowTooltip()
    {
        tooltipPanel.ShowTooltip(inventoryData.InventoryItems[slotIndex], inventoryData);
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
    #endregion
}
