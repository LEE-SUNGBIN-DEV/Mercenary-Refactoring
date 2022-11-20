using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class EquipmentSlot : BaseSlot
{
    private Character character;

    public void Initialize(Character targetCharacter)
    {
        base.Initialize();
        character = targetCharacter;
    }

    #region Equip & Release Function
    public void EquipItem<T>(T item) where T: EquipmentItem
    {
        T equipItem = item as T;
        if (equipItem != null)
        {
            SetItemToSlot(equipItem);
            equipItem.Equip(character.Status);
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
            equipItem.Equip(character.Status);
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
        T equipmentItem = Managers.UIManager.SlotController.DragSlot.Item as T;
        if (equipmentItem != null)
        {
            ReleaseItem<T>();
            EquipItem(Managers.UIManager.SlotController.DragSlot.Item as T);
        }
    }
    public void EndDragEquipmentSlot<T>() where T : EquipmentItem
    {
        T equipmentItem = Managers.UIManager.SlotController.DragSlot.Item as T;
        if (equipmentItem != null)
        {
            ReleaseItem<T>();
            EquipItem(Managers.UIManager.SlotController.TargetSlot.Item as T);
        }
        else if (Managers.UIManager.SlotController.TargetSlot is IAllItemAcceptableSlot
            && Managers.UIManager.SlotController.TargetSlot.Item == null)
        {
            ReleaseItem<T>();
            ClearSlot();
        }
    }
    #endregion
}
