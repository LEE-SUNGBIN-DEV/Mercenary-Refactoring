using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class ArmorSlot : EquipmentSlot
{
    public override void Drop()
    {
        DropEquipmentSlot<ArmorItem>();
    }

    public override void EndDrag()
    {
        EndDragEquipmentSlot<ArmorItem>();
    }

    public override void SlotRightClick(PointerEventData eventData)
    {
        ReleaseItem<ArmorItem>();
    }
}
