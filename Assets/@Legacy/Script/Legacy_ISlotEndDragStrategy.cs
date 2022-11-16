using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// ======================================
//              Legacy Script
// ======================================

/*
public interface Legacy_ISlotEndDragStrategy
{
    public void Action(Legacy_Slot slot);
}

public class SlotEndDragCancel : Legacy_ISlotEndDragStrategy
{
    public void Action(Legacy_Slot slot)
    {
        DragSlot.Instance.ClearDragSlot();
        return;
    }
}

public class SlotEndDragExChange : Legacy_ISlotEndDragStrategy
{
    public void Action(Legacy_Slot slot)
    {
        slot.AddItemToSlot(DragSlot.Instance.Item, DragSlot.Instance.ItemCount);
        DragSlot.Instance.ClearDragSlot();
        return;
    }
}

public class SlotEndDragCombine : Legacy_ISlotEndDragStrategy
{
    public void Action(Legacy_Slot slot)
    {
        slot.ClearSlot();
        DragSlot.Instance.ClearDragSlot();
        return;
    }
}
*/