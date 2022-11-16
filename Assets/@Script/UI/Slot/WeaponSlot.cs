using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class WeaponSlot : EquipmentSlot
{
    public override void ClickSlot(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public override void Drop()
    {
        throw new System.NotImplementedException();
    }

    public override void EndDrag()
    {
        throw new System.NotImplementedException();
    }

    public override void EquipItem<T>(T item)
    {
        throw new System.NotImplementedException();
    }

    public override void ReleaseItem()
    {
        throw new System.NotImplementedException();
    }
}
