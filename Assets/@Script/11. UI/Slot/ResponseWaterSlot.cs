using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ResponseWaterSlot : BaseItemSlot, ITooltipItemSlot, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image responseWaterImageFrame;
    [SerializeField] private ItemTooltipPanel tooltipPanel;

    public ItemTooltipPanel TooltipPanel { get { return tooltipPanel; } set { tooltipPanel = value; } }

    public override void Initialize(int slotIndex = 0)
    {
        base.Initialize(slotIndex);
        responseWaterImageFrame = Functions.FindChild<Image>(gameObject, "Response_Water_Image_Frame", true);
        responseWaterImageFrame.sprite = Managers.ResourceManager.LoadResourceSync<Sprite>(Constants.SPRITE_THUMBNAIL_RESPONSE_WATER_FRAME);
    }

    public override void UpdateSlot(CharacterInventoryData inventoryData)
    {
        base.UpdateSlot(inventoryData);
        ResponseWaterItem item = inventoryData.ResponseWaterSlotItem;
        if (item != null)
        {
            itemImage.sprite = item.GetItemSprite();
            itemImage.color = new Color32(255, 255, 255, 255);
            itemImage.fillAmount = inventoryData.GetRemainingResponseWaterRatio();
            HideAmountText();
            HideGradeText();
        }
    }

    public void ShowTooltip()
    {
        tooltipPanel.ShowTooltip(inventoryData.ResponseWaterSlotItem, inventoryData);
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
