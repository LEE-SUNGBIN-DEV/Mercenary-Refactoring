using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class HelmetSlot : EquipmentSlot
{
    public override void Drop()
    {
        DropEquipmentSlot<HelmetItem>();
    }

    public override void EndDrag()
    {
        EndDragEquipmentSlot<HelmetItem>();
    }

    public override void SlotRightClick(PointerEventData eventData)
    {
        ReleaseItem<HelmetItem>();
    }
}
