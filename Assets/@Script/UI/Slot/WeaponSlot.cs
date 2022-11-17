using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class WeaponSlot : EquipmentSlot
{
    public override void Drop()
    {
        DropEquipmentSlot<WeaponItem>();
    }

    public override void EndDrag()
    {
        EndDragEquipmentSlot<WeaponItem>();
    }

    public override void SlotRightClick(PointerEventData eventData)
    {
        ReleaseItem<WeaponItem>();
    }
}
