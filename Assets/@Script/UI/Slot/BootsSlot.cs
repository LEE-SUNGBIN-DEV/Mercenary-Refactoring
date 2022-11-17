using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class BootsSlot : EquipmentSlot
{
    public override void Drop()
    {
        DropEquipmentSlot<BootsItem>();
    }

    public override void EndDrag()
    {
        EndDragEquipmentSlot<BootsItem>();
    }

    public override void SlotRightClick(PointerEventData eventData)
    {
        ReleaseItem<BootsItem>();
    }
}
