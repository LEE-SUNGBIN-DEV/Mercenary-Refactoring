using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : BaseSlot, IAllItemAcceptableSlot
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void SlotRightClick(PointerEventData eventData)
    {

    }

    public override void Drop()
    {
        throw new System.NotImplementedException();
    }

    public override void EndDrag()
    {
        throw new System.NotImplementedException();
    }
}
