using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EquipmentSlot : BaseSlot
{
    public override void Initialize()
    {
        base.Initialize();
    }

    #region Equip & Release Function
    public void EquipItem<T>(T item) where T: EquipmentItem
    {
        T equipItem = item as T;
        if (equipItem != null)
        {
            SetItemToSlot(equipItem);
            equipItem.Equip(Managers.DataManager.CurrentCharacter);
        }
        else
        {
            Debug.Log("¿Â¬¯ ΩΩ∑‘¿Ã ¥Ÿ∏®¥œ¥Ÿ.");
        }
    }
    public void EquipItem<T>() where T : EquipmentItem
    {
        EquipItem(item as T);
    }

    public void ReleaseItem<T>(T item) where T: EquipmentItem
    {
        T equipItem = item as T;
        if (equipItem != null)
        {
            SetItemToSlot(equipItem);
            equipItem.Equip(Managers.DataManager.CurrentCharacter);
        }
        else
        {
            Debug.Log("¿Â¬¯ ΩΩ∑‘¿Ã ¥Ÿ∏®¥œ¥Ÿ.");
        }
    }
    public void ReleaseItem<T>() where T: EquipmentItem
    {
        ReleaseItem(item as T);
    }
    #endregion

    #region Mouse Event Function
    public void DropEquipmentSlot<T>() where T : EquipmentItem
    {
        T equipmentItem = Managers.SlotManager.DragSlot.Item as T;
        if (equipmentItem != null)
        {
            ReleaseItem<T>();
            EquipItem(Managers.SlotManager.DragSlot.Item as T);
        }
    }
    public void EndDragEquipmentSlot<T>() where T : EquipmentItem
    {
        T equipmentItem = Managers.SlotManager.DragSlot.Item as T;
        if (equipmentItem != null)
        {
            ReleaseItem<T>();
            EquipItem(Managers.SlotManager.TargetSlot.Item as T);
        }
        else if (Managers.SlotManager.TargetSlot is IAllItemAcceptableSlot
            && Managers.SlotManager.TargetSlot.Item == null)
        {
            ReleaseItem<T>();
            ClearSlot();
        }
    }
    #endregion
}
