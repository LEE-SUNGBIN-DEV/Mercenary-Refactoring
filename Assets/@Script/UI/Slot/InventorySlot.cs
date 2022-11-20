using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventorySlot : BaseSlot, IAllItemAcceptableSlot
{
    public void Initialize(int i)
    {
        base.Initialize();
        slotIndex = i;
    }

    public override void SlotRightClick(PointerEventData eventData)
    {
    }

    public override void Drop()
    {
        Managers.DataManager.SelectCharacterData.InventoryData.AddItemBySlot(Managers.UIManager.SlotController.TargetSlot);
    }

    public override void EndDrag()
    {
        Managers.DataManager.SelectCharacterData.InventoryData.AddItemBySlot(Managers.UIManager.SlotController.SelectSlot);
    }
}
