using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class HelmetSlot : EquipmentSlot
{
    public override void EquipItem<T>(T item)
    {
        HelmetItem helmet = item as HelmetItem;
        if (helmet != null)
        {
            AddItemToSlot(helmet);
            helmet.Equip(Managers.DataManager.CurrentCharacter);
            isEquip = true;
        }
        else
        {
            Debug.Log("장착 슬롯이 다릅니다.");
        }
    }

    public override void ReleaseItem()
    {
        HelmetItem helmet = item as HelmetItem;
        if (helmet != null)
        {
            ClearSlot();
            helmet.Release(Managers.DataManager.CurrentCharacter);
            isEquip = false;
        }
        else
        {
            Debug.Log("장착 중인 장비가 없습니다.");
        }
    }

    public override void Drop()
    {
        if(Managers.SlotManager.DragSlot.Item is HelmetItem)
        {
            ReleaseItem();
            EquipItem(Managers.SlotManager.DragSlot.Item);
        }
    }

    public override void EndDrag()
    {
        if (Managers.SlotManager.TargetSlot.Item is HelmetItem)
        {
            ReleaseItem();
            EquipItem(Managers.SlotManager.TargetSlot.Item);
        }
        else if(Managers.SlotManager.TargetSlot.Item == null)
        {
            ReleaseItem();
            ClearSlot();
        }
    }

    public override void ClickSlot(PointerEventData eventData)
    {
        ReleaseItem();
    }
}
