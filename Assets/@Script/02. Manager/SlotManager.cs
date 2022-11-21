using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotManager
{
    private BaseSlot selectSlot;
    private BaseSlot targetSlot;
    private Image dragImage;

    public void Initialize(GameObject rootObject)
    {
        selectSlot = null;
        targetSlot = null;

        dragImage = Functions.FindChild<Image>(rootObject, "DragImage", true);
        dragImage.raycastTarget = false;
        DisableDragImage();
    }

    public void EnableDragImage(Sprite itemSprite)
    {
        dragImage.sprite = itemSprite;
        dragImage.color = Functions.SetColor(dragImage.color, 0.7f);
        dragImage.gameObject.SetActive(true);
    }
    public void DisableDragImage()
    {
        dragImage.sprite = null;
        dragImage.color = Functions.SetColor(dragImage.color, 0f);
        dragImage.gameObject.SetActive(false);
    }

    public void OnBeginDrag<T>(T slot) where T : BaseSlot
    {
        selectSlot = slot;
        targetSlot = null;

        EnableDragImage(selectSlot.ItemImage.sprite);
    }
    public void OnDrag(PointerEventData eventData)
    {
        dragImage.rectTransform.position = eventData.position;
    }
    public void OnDrop<T>(T slot) where T : BaseSlot
    {
        targetSlot = slot;

    }
    public void OnEndDrag()
    {
        DisableDragImage();
        selectSlot = null;
        targetSlot = null;
    }

    #region Property
    public BaseSlot SelectSlot { get { return selectSlot; } }
    public BaseSlot TargetSlot { get { return targetSlot; } }
    #endregion
}
