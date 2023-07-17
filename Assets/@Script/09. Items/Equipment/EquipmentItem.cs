using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class EquipmentItem : BaseItem
{
    [Header("Equipment Item")]
    [SerializeField] protected int grade;

    public override void Initialize(ItemData itemData)
    {
        if (CheckItemTable(itemData))
        {
            base.Initialize(itemData);
            grade = itemData.grade;
        }
    }
    public override void Initialize<T>(T item)
    {
        base.Initialize(item);
        if (item is EquipmentItem targetItem)
        {
            grade = targetItem.grade;
        }
    }

    public abstract void Equip(CharacterStatusData _status);
    public abstract void UnEquip(CharacterStatusData _status);

    public int Grade { get { return grade; } set { grade = value; } }
}
