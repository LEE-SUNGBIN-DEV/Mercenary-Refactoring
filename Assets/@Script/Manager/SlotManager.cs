using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotManager
{
    private BaseSlot selectSlot;
    private BaseSlot targetSlot;
    private BaseSlot dragSlot;

    public void Initialize()
    {
        selectSlot = null;
        targetSlot = null;

        //GameObject slotObject = Managers.ResourceManager.InstantiatePrefabSync("Prefab_Drag_Slot");
        //temporarySlot = slotObject.GetComponent<BaseSlot>();
        //temporarySlot.gameObject.SetActive(false);
    }

    public void OnBeginDrag<T>(T slot) where T : BaseSlot
    {
        selectSlot = slot;
        targetSlot = null;

        dragSlot.gameObject.SetActive(true);
        dragSlot.SetSlot(selectSlot);
    }
    public void OnDrag(PointerEventData eventData)
    {
        dragSlot.ItemImage.rectTransform.position = eventData.position;
    }
    public void OnDrop<T>(T slot) where T : BaseSlot
    {
        targetSlot = slot;
    }
    public void OnEndDrag()
    {
        dragSlot.ClearSlot();
        dragSlot.gameObject.SetActive(false);

        selectSlot = null;
        targetSlot = null;
    }

    #region Property
    public BaseSlot SelectSlot { get { return selectSlot; } }
    public BaseSlot TargetSlot { get { return targetSlot; } }
    public BaseSlot DragSlot { get { return dragSlot; } }
    #endregion
}
