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

    public void EquipItem<T>(T item) where T: EquipmentItem
    {
        T equipItem = item as T;
        if (equipItem != null)
        {
            SetSlotByItem(equipItem);
            equipItem.Equip(character);
        }
        else
        {
            Debug.Log("¿Â¬¯ ΩΩ∑‘¿Ã ¥Ÿ∏®¥œ¥Ÿ.");
        }
    }

    public abstract void UnEquipItem();

    #region Mouse Event Function
    public void DropEquipmentSlot<T>() where T : EquipmentItem
    {
    }
    public void EndDragEquipmentSlot<T>() where T : EquipmentItem
    {
    }
    #endregion
}
